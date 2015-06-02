using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbRepositories
{
    public class ScampSubscription
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "azureSubscriptionId")]
        public string AzureSubscriptionID { get; set; }
        [JsonProperty(PropertyName = "azureAdminUser")]
        public string AzureAdminUser { get; set; }
        [JsonProperty(PropertyName = "azureAdminPassword")]
        public string AzureAdminPassword { get; set; }
        [JsonProperty(PropertyName = "azureManagementThumbnail")]
        public string AzureManagementThumbnail { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "subscription"; } }

    }
}
