using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureProvisioningLibrary
{
    public static class ProvisioningLibraryConfiguration
    {
        public static string GetStorageConnectionString()
        {
            return "DefaultEndpointsProtocol=https;AccountName=[AccountName];AccountKey=[AccountKey]";
        }

        public static string GetStorageSubscriptionCertificate()
        {
            return "{add certificate}";
        }
        public static string GetStorageSubscriptionId()
        {
            return "{add Subscription Id}";
        }
    }
}
