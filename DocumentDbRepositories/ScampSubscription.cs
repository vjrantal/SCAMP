using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbRepositories
{
    public class ScampSubscription : Resource
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "azureSubscriptionId")]
        public string AzureSubscriptionID { get; set; }
        [JsonProperty(PropertyName = "azureAdminUser")]
        public string AzureAdminUser { get; set; }
        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }
        [JsonProperty(PropertyName = "azureManagementThumbnail")]
        public string AzureManagementThumbnail { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "subscription"; } }

    }
}
