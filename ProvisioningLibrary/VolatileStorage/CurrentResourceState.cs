using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ProvisioningLibrary
{

    public class CurrentResourceState : TableEntity
    {
        private string _ResourceId = string.Empty;
        public static string PKey = "ResourceState";
        
        public CurrentResourceState(string resourceId)
        {
            this.RowKey = resourceId;
            this.PartitionKey = CurrentResourceState.PKey + this.RowKey;
        }

        public string ResourceId {
            get
            {
                return this.RowKey;
            }
        }
        public int State { get; set; }
        public long UnitsUsed { get; set; }
        public DateTime NextConsolidation { get; set; }
    }
}
