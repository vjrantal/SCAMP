using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbRepositories
{
    public class ScampUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "user"; } }

        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }

        [JsonProperty(PropertyName = "IPKey")]
        public string IPKey { get; set; }

        [JsonProperty(PropertyName = "IsSystemAdmin")]
        public bool IsSystemAdmin { get; set; }
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