using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using ScampTypes.ViewModels;

namespace DocumentDbRepositories
{
    [Serializable]
    public class ScampUserBudget
    {
        [JsonProperty(PropertyName = "unitsBudgeted")]
        public long unitsBudgeted { get; set; }
        [JsonProperty(PropertyName = "allocated")]
        public long Allocated { get; set; }
        [JsonProperty(PropertyName = "enddate")]
        public DateTime EndDate { get; set; }
    }

    public class ScampGroupBudget : ScampUserBudget
    {
        [JsonProperty(PropertyName = "ownerId")]
        public string OwnerId { get; set; }
        [JsonProperty(PropertyName = "ownerName")]
        public string OwnerName { get; set; }
        [JsonProperty(PropertyName = "defaultUserAllocation")]
        public long DefaultUserAllocation { get; set; } 
    }
}