using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace ScampApi.ViewModels
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "isSystemAdmin")]
        public bool IsSystemAdmin { get; set; }

        [JsonProperty(PropertyName = "groups")]
        public IEnumerable<GroupSummary> Groups { get; internal set; }

        [JsonProperty(PropertyName = "resources")]
        public IEnumerable<ScampResourceSummary> Resources { get; internal set; }
    }
}