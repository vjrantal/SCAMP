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
            Admins = new List<ScampUserReference>();
            Members = new List<ScampUserReference>();
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "admins")]
		public List<ScampUserReference> Admins { get; set; }
        [JsonProperty(PropertyName = "members")]
		public List<ScampUserReference> Members { get; set; }
        [JsonProperty(PropertyName = "budget")]
        public ScampUserBudget Budget { get; set; }

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
        [JsonProperty(PropertyName = "admins")]
        public List<ScampUserReference> Admins { get; set; }
        [JsonProperty(PropertyName = "members")]
        public List<ScampUserReference> Members { get; set; }
        [JsonProperty(PropertyName = "budget")]
        public ScampGroupBudget Budget { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "group"; } }

    }
}
