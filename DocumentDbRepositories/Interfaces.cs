using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DocumentDbRepositories
{
    public interface IUserRepository
    {
        Task CreateUser(ScampUser newUser);
        Task<ScampUser> GetUserbyId(string userId);
        Task UpdateUser(ScampUser user);
    }

    public interface IGroupRepository
    {
        Task<ScampResourceGroup> GetGroup(string groupID);
        Task<ScampResourceGroupWithResources> GetGroupWithResources(string groupID);
        Task<IEnumerable<ScampResourceGroup>> GetGroups();
        Task<IEnumerable<ScampResourceGroup>> GetGroupsByUser(ScampUserReference user);
        Task CreateGroup(ScampResourceGroup newGroup);
        Task UpdateGroup(string groupID, ScampResourceGroup group);
    }

    public interface IResourceRepository
    {
        Task<ScampResource> GetResource(string resourceId);
        Task<IEnumerable<ScampResource>> GetResources();
        Task<List<ScampResourceReference>> GetResources(List<string> resourceIds);
        Task<IEnumerable<ScampResource>> GetResourcesByOwner(string userId);
        Task CreateResource(ScampResource resource);
        Task<IEnumerable<ScampResource>> GetResourcesByGroup(ScampUserReference user, string groupId);
        Task UpdateResource(ScampResource resource);
        Task DeleteResource(string resourceId);
    }

    public interface ISubscriptionRepository
    {
        Task CreateSubscription(ScampSubscription newSubscription);
        Task<ScampSubscription> GetSubscription(string subscriptionId);
        Task<List<ScampSubscription>> GetSubscriptions();
    }

    public interface ISystemSettingsRepository
    {
        Task<List<ScampUser>> GetSystemAdministrators();
        Task<StyleSettings> GetSiteStyleSettings();
    }
}