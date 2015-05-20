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
    public class ScampUser : Resource
    {
        public ScampUser()
        {
            GroupMembership = new List<ScampUserGroupMbrship>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "user"; } }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "isSystemAdmin")]
        public bool IsSystemAdmin { get; set; }

        [JsonProperty(PropertyName = "groupmbrship")]
        public List<ScampUserGroupMbrship> GroupMembership { get; set; }
    }

    public class ScampUserGroupMbrship
    {
        public ScampUserGroupMbrship()
        {
            Resources = new List<ScampUserGroupResources>();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isAdmin")]
        public bool isAdmin { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<ScampUserGroupResources> Resources { get; set; }
    }

    public class ScampUserGroupResources
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public ResourceType type { get; set; }

        [JsonProperty(PropertyName = "state")]
        public ResourceState state { get; set; }
    }

    public class ScampUserReference
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public static implicit operator ScampUserReference(ScampUser user)
        {
            return new ScampUserReference { Id = user.Id, Name = user.Name };
        }
    }
}