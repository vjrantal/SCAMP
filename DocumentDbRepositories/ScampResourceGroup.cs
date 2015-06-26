using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Documents;

namespace DocumentDbRepositories
{
    /// <summary>
    /// This maps to the underlying document model for groups
    /// </summary>
    public class ScampResourceGroup : Resource
    {
        public ScampResourceGroup()
        {
            Members = new List<ScampUserGroupMbrship>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "members")]
		public List<ScampUserGroupMbrship> Members { get; set; }
        [JsonProperty(PropertyName = "budget")]
        public ScampGroupBudget Budget { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "group"; } }

    }
}
