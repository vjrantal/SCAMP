using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

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