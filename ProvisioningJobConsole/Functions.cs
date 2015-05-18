using System;
using System.Diagnostics;
using System.IO;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using KeyVaultRepositories.Implementation;
using Microsoft.Azure.WebJobs;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Fallback;
using Microsoft.WindowsAzure.Management.Compute.Models;
using ProvisioningLibrary;
using System.Threading.Tasks;
using ScampTypes.ViewModels;

namespace ProvisioningJobConsole
{
    public class Functions
    {
        static IServiceProvider Provider = null;

        static Functions()
        {
            // Setup configuration sources.
            var configuration = new Configuration()
                 .AddEnvironmentVariables("APPSETTING_");

            var services = new ServiceCollection();
            services.AddDocumentDbRepositories(configuration);
            services.AddKeyVaultRepositories(configuration);
            services.AddProvisioning(configuration);

            services.UseVolatileStorage(configuration);


            Provider = services.BuildServiceProvider();
        }

        class ResourceActivity
        {
            public IResourceController ResourceController { get; private set; }
            public ResourceActivity(IResourceController rc)
            {
                this.ResourceController = rc;
            }

            public ScampResource Resource { get; private set; }

            public ScampSubscription Subscription { get; private set; }

            public bool IsCreating { get; private set; }

            public string ServiceName { get; private set; }

            public string MachineName { get { return Resource.Name; } }

            public ProvisioningController Provisioning { get; private set; }

            public async Task<bool> TryInitializeAsync(string id)
            {
                Console.WriteLine("Retrieving Resource: " + id);
                Resource = await ResourceController.GetResource(id);
                if (Resource == null)
                    return false;

                if (string.IsNullOrEmpty(Resource.SubscriptionId))
                {
                    //need to create a VM
                    Console.WriteLine("Creating VM");
                    this.IsCreating = true;

                    Subscription = await ResourceController.GetAvailabeDeploymentSubscription();
                    ServiceName = await ResourceController.GetCloudServiceName(Resource);
                }
                else
                {
                    Console.WriteLine("Retrieving Subscription: " + Resource.SubscriptionId);
                    Subscription = await ResourceController.GetSubscription(Resource.SubscriptionId);
                    ServiceName = Resource.CloudServiceName;
                }


                Provisioning = new ProvisioningController(Subscription.AzureManagementThumbnail, Subscription.AzureSubscriptionID);
                return true;
            }
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public async static void ProcessQueueMessage([QueueTrigger("processorqueue")] QueueMessage message, TextWriter log)
        {
            Console.WriteLine(string.Format("Process Q Message: Resource[{0}] Action[{1}] RequestId[{2}]", 
                                            message.ResourceId, 
                                            message.Action,
                                            message.OperationGuid.ToString()));

            Console.WriteLine(string.Format("Start - Current Time: [{0}]", 
                                             DateTime.UtcNow.ToLongTimeString()));
            // TODO - shouldn't be depending on ResourceController here
            var resourceController = Provider.GetService<IResourceController>();
            var volatileStorageController = Provider.GetService<IVolatileStorageController>();
            var activity = new ResourceActivity(resourceController);



            

           

            if (!await activity.TryInitializeAsync(message.ResourceId))
            {
                Console.WriteLine("Resource not found");
                return;
            }

            if (activity.IsCreating)
                message.Action = ResourceAction.Create;

            // sometimes actions will throw exceptions, handle them gracefully:
            // example:
            // ConflictError: Another reboot or reimage operation is already in progress on role instance brent1.

            try
            {
                
                switch (message.Action)
                {
                    case ResourceAction.Delete:
                        await ActionDelete(activity);
                        break;
                    case ResourceAction.Stop:
                        await ActionStop(activity);
                        break;
                    case ResourceAction.Start:
                        await ActionStart(activity);
                        break;
                    case ResourceAction.Create:
                        await ActionCreate(activity);
                        break;
                    default:
                        Console.WriteLine("Unhandled action: " + message.Action);
                        return;
                }


                /*

                the following is logging code. it will need to 
                be uncommented after the previous code is stable enough

                */

                var groupId = string.Empty;
                if (activity.Resource != null &&
                    activity.Resource.ResourceGroup != null)
                    groupId = activity.Resource.ResourceGroup.Id;



                await volatileStorageController.CreateActivityLog(new ActivityLog()
                {
                    ResourceId = message.ResourceId,
                    GroupId = groupId,
                    RequestId = message.OperationGuid.ToString(),
                    Action = message.Action.ToString(),
                    UserId = "ProvisioningConsole"
                });


                

                //Console.WriteLine(string.Format("My orginal message was: {0}", message));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Process Q Message FAILED!: Resource[{0}] Action[{1}] RequestId[{2}] Error[{3}]",
                                         message.ResourceId,
                                         message.Action,
                                         message.OperationGuid.ToString(), e.Message));
            }

            Console.WriteLine(string.Format("End - Current Time: [{0}]",
                             DateTime.UtcNow.ToLongTimeString()));

        }

        static async Task ActionDelete(ResourceActivity activity)
        {

            //TODO Temporary
            try
            {
                await activity.Provisioning.StartStopVirtualMachineAsync(activity.MachineName, activity.ServiceName, VirtualMachineAction.Stop);
            }
            catch
            {
                Console.WriteLine(string.Format("Stop VM (to delete) [{0}] failed - machine might be stopping or stopped", activity.MachineName));
            }

            Console.WriteLine(string.Format("about to delete VM [{0}]", activity.MachineName));
            await activity.ResourceController.DeleteResource(activity.Resource);
            Console.WriteLine(string.Format("Deleted VM [{0}]", activity.MachineName));

        }

        static async Task ActionStop(ResourceActivity activity)
        {
                
                Console.WriteLine(string.Format("Stopping VM [{0}]", activity.MachineName));
                await activity.Provisioning.StartStopVirtualMachineAsync(activity.MachineName, activity.ServiceName, VirtualMachineAction.Stop);
                activity.Resource.State = ResourceState.Stopping;
                await activity.ResourceController.UpdateResource(activity.Resource);
        }


        static async Task ActionStart(ResourceActivity activity)
        {
            Console.WriteLine(string.Format("Starting VM [{0}]", activity.MachineName));
            await activity.Provisioning.StartStopVirtualMachineAsync(activity.MachineName, activity.ServiceName, VirtualMachineAction.Start);
            activity.Resource.State = ResourceState.Starting;
            await activity.ResourceController.UpdateResource(activity.Resource);
        }

        static async Task ActionCreate(ResourceActivity activity)
        {
            var r = new Random();
            //Need to find a way to assign Usename and Pwd
            string username = "ScampAdmin";
            string password = "Enter.321";
            string storageAccountName = activity.ServiceName;
            int rdpPort = r.Next(3000, 4000);

            activity.Resource.State = ResourceState.Starting;
            await activity.ResourceController.UpdateResource(activity.Resource);

            var isCloudServiceAlreadyCreated =
                            await activity.Provisioning.IsCloudServiceAlreadyCreated(activity.ServiceName);

            if (!isCloudServiceAlreadyCreated)
            {
                Console.WriteLine(string.Format("Creating Cloud Services [{0}]", activity.ServiceName));

                string location = activity.ResourceController.GetServiceLocation();
                var x = activity.Provisioning.CreateCloudService(activity.ServiceName, location);
                x.Wait();
                Console.WriteLine(string.Format("Creating Storage for Cloud Service [{0}]", activity.ServiceName));

                await activity.Provisioning.CreateStorageAccount(location, storageAccountName);
            }

            //TODO Need to delete disk in case is already there

            Console.WriteLine(string.Format("Creating Virtual Machine [{0}]", activity.ServiceName));


            var z = await activity.Provisioning.CreateVirtualMachine(activity.MachineName, activity.ServiceName, storageAccountName,
                username, password, "Visual-Studio-2015-Ultimate", VirtualMachineRoleSize.Small, rdpPort, isCloudServiceAlreadyCreated);

            activity.Resource.CloudServiceName = activity.ServiceName;

            activity.Resource.SubscriptionId = activity.Subscription.Id;
            activity.Resource.UserName = username;

            //docDbResource.UserPassword = password;
            activity.Resource.RdpPort = rdpPort.ToString();

            var vault = Provider.GetService<IKeyRepository>();
            await vault.UpsertSecret(activity.Resource.Id, "password", password);

            await activity.ResourceController.UpdateResource(activity.Resource);
        }

    }

}
