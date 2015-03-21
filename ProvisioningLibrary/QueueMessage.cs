using System;
using ProvisioningLibrary;

namespace ProvisioningLibrary
{
    public class QueueMessage
    {
        public Guid OperationGuid { get; set; }
        public string ResourceId { get; set; }
        public ResourceAction Action { get; set; }
    }
}
