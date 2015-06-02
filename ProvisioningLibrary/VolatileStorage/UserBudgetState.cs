using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ProvisioningLibrary
{

    public class UserBudgetState : TableEntity
    {
        private string _ResourceId = string.Empty;
        public static string PKey = "ResourceState";
        
        public UserBudgetState()
        {

        }

        public UserBudgetState(string userId, string groupId) : this()
        {
            this.PartitionKey = userId;
            this.RowKey = groupId;
        }

        public string userId
        {
            get
            {
                return this.PartitionKey;
            }
        }

        public string groupId {
            get
            {
                return this.RowKey;
            }
        }
        public double UnitsBudgetted { get; set; }
        public double UnitsUsed { get; set; }
    }
}
