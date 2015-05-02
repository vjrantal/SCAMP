using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbRepositories
{
    public enum ResourceType {
        VirtualMachine = 1, // Azure Virtual Machine
        WebApp = 2, // Azure Web App
    };

    public enum ResourceState
    {
        Allocated = 0, // the resource can be created, but doesn't yet exist
        Starting = 1, // the resource is being started, if its the first time, this also means provisioning
        Running = 2, // the resource is running/active
        Stopping = 3, // the resource is being stopped
        Stopped = 4, // the resource is stopped, but may still be incuring charges
        Suspended = 5, // the resource has exceeded its usage quota
        Deleting = 6 // the resource is being deleted, when complete it will be in an allocated state
    };

    public class ScampResource
    {
        public ScampResource()
        {
            Owners = new List<ScampUserReference>();
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "resourceGroup")]
        public ScampResourceGroupReference ResourceGroup { get; set; }
        [JsonProperty(PropertyName = "azureResourceId")]
        public string AzureResourceId { get; set; }
        [JsonProperty(PropertyName = "subscriptionId")]
        public string SubscriptionId { get; set; }
        [JsonProperty(PropertyName = "resourceType")]
        public ResourceType ResourceType { get; set; }
        [JsonProperty(PropertyName = "state")]
        public ResourceState State { get; set; }
        [JsonProperty(PropertyName = "cloudServiceName")]
        public string CloudServiceName { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "userPassword")]
        public string UserPassword { get; set; }
        [JsonProperty(PropertyName = "rdpPort")]
        public string RdpPort { get; set; }

        [JsonProperty(PropertyName = "owners")]
        public List<ScampUserReference> Owners { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "resource"; } }
    }

    public class ScampResourceReference
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "resourceGroup")]
        public ScampResourceGroupReference ResourceGroup { get; set; }
        [JsonProperty(PropertyName = "state")]
        public int State { get; set; }
    }
}
