using Newtonsoft.Json;
using System.Collections.Generic;

namespace DocumentDbRepositories
{
    /// <summary>
    /// This maps to the underlying document model for groups
    /// </summary>
    public class ScampResourceGroup
    {
        public ScampResourceGroup()
        {
            Members = new List<ScampUserGroupMbrship>();
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
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

    /// <summary>
    /// This includes resources for the group and is used for querying.
    /// </summary>
    public class ScampResourceGroupWithResources
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
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
