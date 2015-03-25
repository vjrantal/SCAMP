using System;
using System.Data.Services.Client;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using Microsoft.Azure.WebJobs;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using ProvisioningLibrary;
using ProvisioningLibrary5x;

namespace ProvisioningJobConsole
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public async  static void ProcessQueueMessage([QueueTrigger("processorqueue")] QueueMessage message, TextWriter log)
        {

            // Setup configuration sources.
           var configuration = new Configuration()
                .AddEnvironmentVariables("APPSETTING_");
            
            //TODO Enable Dependency Injection
            var resourceController = new ResourceController(new RepositoryFactory(configuration));

            var docDbResource = await resourceController.GetResource(message.ResourceId);
            ScampSubscription subscription;
            string cloudServiceName, machineName;




            if (string.IsNullOrEmpty(docDbResource.SubscriptionId))
            {
                //need to create a VM
                log.WriteLine("Creating VM");

                subscription = await resourceController.GetAvailabeDeploymentSubscription();
                message.Action = ResourceAction.Create;
                cloudServiceName = await resourceController.GetCloudServiceName(docDbResource);
                machineName = docDbResource.Name;
            }
            else
            {
                subscription = await resourceController.GetSubscription(docDbResource.SubscriptionId);
                machineName = docDbResource.Name;
                cloudServiceName = docDbResource.CloudServiceName;
            }
        
            var provisioningController = new ProvisioningController(subscription.AzureManagementThumbnail , subscription.AzureSubscriptionID);
            
           


            //TODO Get Connection string from DB

            if (message.Action == ResourceAction.Stop )
            {
                log.WriteLine("Stopping VM");
                var x=provisioningController.StartStopVirtualMachine(machineName ,cloudServiceName , VirtualMachineAction.Stop);
                x.Wait();
            }
            if (message.Action == ResourceAction.Start)
            {
                log.WriteLine("Starting VM");
               var x=provisioningController.StartStopVirtualMachine(machineName, cloudServiceName, VirtualMachineAction.Start);
                x.Wait();
            }
            if (message.Action == ResourceAction.Create )
            {
                var r = new Random();
                string username = "gabrielc";
                string password = "Enter.321";
                string location = resourceController.GetServiceLocation();
                string storageAccountName = resourceController.GetStorageAccountName();
                int rdpPort = r.Next(3000, 4000);

               
                if(! await provisioningController.IsCloudServiceAlreadyCreated(cloudServiceName))
                {
                    log.WriteLine("Creating Cloud Services");
                    var x= provisioningController.CreateCloudService(cloudServiceName, location);
                    x.Wait();
                    log.WriteLine("Creating Storage");
                    var y = provisioningController.CreateStorageAccount(location, storageAccountName);
                    y.Wait();
                }
                log.WriteLine("Creating Virtual Machine");
                var z = provisioningController.CreateVirtualMachine(machineName, cloudServiceName, storageAccountName,
                    username, password, "Visual-Studio-2015-Ultimate", VirtualMachineRoleSize.Small, rdpPort);
                z.Wait();
                docDbResource.CloudServiceName = cloudServiceName;
                docDbResource.State = "Created";
                docDbResource.SubscriptionId = subscription.Id;
                docDbResource.UserName = username;
                docDbResource.UserPassword = password;
                docDbResource.RdpPort = rdpPort.ToString();
                await resourceController.UpdateResource(docDbResource);
            }
          

            log.WriteLine(message);
        }

    }

}
