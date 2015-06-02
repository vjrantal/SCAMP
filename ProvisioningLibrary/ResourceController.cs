using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using KeyVaultRepositories.Implementation;
using Microsoft.WindowsAzure.Management.Models;

namespace ProvisioningLibrary
{
    internal class ResourceController : IResourceController
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ISystemSettingsRepository _settingsRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IKeyRepository _keyRepository;

        public ResourceController(IResourceRepository resourceRepository, ISystemSettingsRepository settingsRepository, IGroupRepository groupRepository, IKeyRepository keyRepository)
        {
            _resourceRepository = resourceRepository;
            _settingsRepository = settingsRepository;
            _groupRepository = groupRepository;
            _keyRepository = keyRepository;
        }

        public async Task<ScampResource> GetResource(string resourceId)
        {
            var res = await _resourceRepository.GetResource(resourceId);
            return res;
        }

        public async Task<ScampSubscription> GetSubscription(string subscriptionId)
        {
            var subscription = await _settingsRepository.GetSubscription(subscriptionId);

            return subscription;
        }

        public async Task<ScampSubscription> GetAvailabeDeploymentSubscription()
        {

            //Need to add the logic of choosing a subscription.
            //For now is the first in the store
            var c = await _settingsRepository.GetSubscriptions();
            var selected = c.LastOrDefault();
            selected.AzureManagementThumbnail = await _keyRepository.GetSecret(selected.Id, "cert");
            return selected;

        }

        public async Task<string> GetCloudServiceName(ScampResource scampResource)
        {
            var grp = await _groupRepository.GetGroupWithResources(scampResource.ResourceGroup.Id);
            return grp.Name.ToLower();
        }

        public string GetServiceLocation()
        {
            return LocationNames.NorthEurope;
        }

        //public string GetStorageAccountName()
        //{
        //    //TODO Find better algorythm
        //    var r = new Random();
        //    return "Scamp" + r.Next(1000,1000) ;
        //}
        public async Task<bool> UpdateResource(ScampResource resource)
        {
            await _resourceRepository.UpdateResource(resource);
            return true;

        }

        public async Task DeleteResource(ScampResource docDbResource)
        {
           await  _resourceRepository.DeleteResource(docDbResource.Id);
        }
    }
}