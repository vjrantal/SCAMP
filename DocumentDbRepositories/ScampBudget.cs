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
        public double unitsBudgeted { get; set; }
        [JsonProperty(PropertyName = "allocated")]
        public double Allocated { get; set; }
        [JsonProperty(PropertyName = "enddate")]
        public DateTime EndDate { get; set; }
    }

    public class ScampGroupBudget : ScampUserBudget
    {
        [JsonProperty(PropertyName = "ownerId")]
        public string OwnerId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "defaultUserAllocation")]
        public double DefaultUserAllocation { get; set; } 
    }
}