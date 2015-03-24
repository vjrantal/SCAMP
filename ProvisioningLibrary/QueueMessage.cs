using System;
using ProvisioningLibrary;

namespace ProvisioningLibrary
{
    public class QueueMessage
    {
        public Guid OperationGuid { get; set; }
        public int ResourceId { get; set; }
        public ResourceAction Action { get; set; }
    }
}
