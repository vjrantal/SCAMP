using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AzureProvisioningLibrary
{
    public class WebJobController
    {
        public WebJobController()
        {
                
        }

        public Guid SubmitActionInQueue(int resourceId, AzureProvisioningLibrary.ResourceAction  action)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ProvisioningLibraryConfiguration.GetStorageConnectionString()); 


            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("processorqueue");


            queue.CreateIfNotExists();
            var actionId = Guid.NewGuid();

            var email = new QueueMessage()
            {
                OperationGuid = actionId,
                Action  = action,
                ResourceId = resourceId
            };


            queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(email)));
            return actionId;
        }
    }
}
