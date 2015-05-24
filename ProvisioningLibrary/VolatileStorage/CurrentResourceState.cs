using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ScampTypes.ViewModels;

namespace ProvisioningLibrary
{

    public class CurrentResourceState : TableEntity
    {
        private string _ResourceId = string.Empty;

        public CurrentResourceState()
        {
        }

        public CurrentResourceState(string resourceId) : this()
        {
            this.RowKey = resourceId;
            this.PartitionKey = this.RowKey;
        }

        public string ResourceId {
            get
            {
                return this.RowKey;
            }
        }
        public ResourceState State { get; set; }
        public long UnitsUsed { get; set; }
        public DateTime NextConsolidation { get; set; }
    }
}
