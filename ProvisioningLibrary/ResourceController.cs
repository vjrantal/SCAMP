using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using Microsoft.WindowsAzure.Management.Models;
using ProvisioningLibrary;

namespace ProvisioningLibrary5x
{
    public class ResourceController
    {
        private readonly RepositoryFactory _repositoryFactory;

        public ResourceController(RepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<ScampResource> GetResource(string resourceId)
        {
            var resRepo = await _repositoryFactory.GetResourceRepositoryAsync();
            var res = await resRepo.GetResource(resourceId);
            return  res;
        }

        public async Task<ScampSubscription> GetSubscription(string subscriptionId)
        {
            var subRepo = await _repositoryFactory.GetSubscriptionRepositoryAsync();
            var subscription= await subRepo.GetSubscription(subscriptionId);
           
            return subscription;
        }

        public async  Task<ScampSubscription> GetAvailabeDeploymentSubscription()
        {

            //Need to add the logic of choosing a subscription.
            //For now is the first in the store
            var subRepo = await _repositoryFactory.GetSubscriptionRepositoryAsync();
            var c= await subRepo.GetSubscriptions();

            return c.FirstOrDefault();

        }

        public async Task<string> GetCloudServiceName(ScampResource  scampResource )
        {
            var groupRepo = await _repositoryFactory.GetGroupRepositoryAsync();
            var grp = await groupRepo.GetGroupWithResources(scampResource.ResourceGroup.Id);
            return grp.Name.ToLower().Replace(" ","-");
        }

        public string GetServiceLocation()
        {
            return LocationNames.NorthEurope;
        }

        public string GetStorageAccountName()
        {
            //TODO Find better algorythm
            var r = new Random();
            return "Scamp-" + r.Next(1000,1000) ;
        }
        public async Task<bool> UpdateResource(ScampResource resource)
        {
            var resRepo = await _repositoryFactory.GetResourceRepositoryAsync();
            resRepo.UpdateResource(resource);
            return true;

        }
    }
}