using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ProvisioningLibrary
{
    public class ActivityLog : TableEntity
    {
        private string _ResourceId = string.Empty;
        private string _RequestId = string.Empty;
        public ActivityLog()
        {
            this.RowKey = DateTime.UtcNow.Ticks.ToString();
        }

        public string RequestId{get;set;}
        public string ResourceId
        {
            get
            {
                return this._ResourceId;
            }
            set
            {
                this._ResourceId = value;
                this.PartitionKey = value;
            }
        }
        public string Action { get; set; }
        public string ToDateUsage { get; set; }
        public int State { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
    }
}
