using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace DocumentDbRepositories
{
    public class StyleSettings : Resource
    {
        [JsonProperty(PropertyName = "settings")]
        public string Settings { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "stylesettings"; } }
    }
}