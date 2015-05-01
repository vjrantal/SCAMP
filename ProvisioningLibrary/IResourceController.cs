using System.Threading.Tasks;
using DocumentDbRepositories;

namespace ProvisioningLibrary
{
    public interface IResourceController
    {
        Task DeleteResource(ScampResource docDbResource);
        Task<ScampSubscription> GetAvailabeDeploymentSubscription();
        Task<string> GetCloudServiceName(ScampResource scampResource);
        Task<ScampResource> GetResource(string resourceId);
        string GetServiceLocation();
        Task<ScampSubscription> GetSubscription(string subscriptionId);
        Task<bool> UpdateResource(ScampResource resource);
    }
}