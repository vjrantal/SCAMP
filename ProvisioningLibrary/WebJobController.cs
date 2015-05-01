using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ProvisioningLibrary
{
    public class WebJobController : IWebJobController
    {
        IDictionary<string, string> _settings;

        public WebJobController(IConfiguration config)
        {
            //TODO Refactor
            IDictionary<string, string> settings = new Dictionary<string, string>();
            var storageCstr = config.Get("Provisioning:StorageConnectionString");
            settings.Add("Provisioning:StorageConnectionString", storageCstr);
            _settings = settings;   
        }

        public Guid SubmitActionInQueue(string  resourceId, ProvisioningLibrary.ResourceAction  action, uint? duration = null)
        {
            var storageConnectionString = _settings["Provisioning:StorageConnectionString"];
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString); 


            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("processorqueue");


            queue.CreateIfNotExists();
            var actionId = Guid.NewGuid();

            var actionMessage    = new QueueMessage()
            {
                OperationGuid = actionId,
                Action  = action,
                ResourceId = resourceId
            };


            queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(actionMessage)));

            // if the action was to start a resource, but a duration was specified
            // send a second queue message with initial visility time out set to
            // 'duration' hours in the future. To trigger automatic stop
            if (action == ResourceAction.Start && duration != null)
            {
                actionMessage.Action = ResourceAction.Stop;
                CloudQueueMessage tmpMsg = new CloudQueueMessage(JsonConvert.SerializeObject(actionMessage));
                queue.AddMessage(tmpMsg, null, new TimeSpan(0, (int)duration, 0),null,null);
            }

            return actionId;
        }

        public static ResourceAction GetAction(string action)
        {
            switch (action.ToLower())
            {
                case "start":
                    return ResourceAction.Start;
                case "stop":
                    return ResourceAction.Stop;
                case "create":
                    return ResourceAction.Create;
                case "delete":
                    return ResourceAction.Delete;
                default:
                    return ResourceAction.Undefined;                
            }
        }

        public void SubmitActionInQueue(string resourceId, string actionname, uint? duration = null)
        {
            ResourceAction action = GetAction(actionname);
            if (action == ResourceAction.Undefined)
                throw new Exception("Action not defined");
 
            this.SubmitActionInQueue(resourceId, action, duration);
        }
    }
}
