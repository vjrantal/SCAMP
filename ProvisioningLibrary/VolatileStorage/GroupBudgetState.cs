using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ProvisioningLibrary
{

    public class GroupBudgetState : TableEntity
    {
        private string _ResourceId = string.Empty;

        public GroupBudgetState()
        {

        }

        public GroupBudgetState(string groupId) : this()
        {
            this.RowKey = groupId;
            this.PartitionKey = groupId;
        }

        public string GroupId {
            get
            {
                return this.PartitionKey;
            }
        }
        public long UnitsBudgetted { get; set; }
        public long UnitsAllocated { get; set; }
        public long UnitsUsed { get; set; }
    }
}
