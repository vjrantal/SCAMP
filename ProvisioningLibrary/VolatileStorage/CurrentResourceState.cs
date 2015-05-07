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
        
        public CurrentResourceState()
        {
            this.PartitionKey = CurrentResourceState.PKey;
        }

        public string ResourceId {
            get
            {
                return this._ResourceId;
            }
            set
            {
                this._ResourceId = value;
                this.RowKey = value;
            }
        }
        public int State { get; set; }
        public string Usage { get; set; }
    }
}
