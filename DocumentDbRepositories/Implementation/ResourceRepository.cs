using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDbRepositories.Implementation
{
    public class ResourceRepository : RepositoryBase
    {
        public Task<ScampResource> GetResource(string resourceId)
        {
            //TODO Check
            var resources = from r in Client.CreateDocumentQuery<ScampResource>(Collection.SelfLink)
                            where r.Id == resourceId && r.Type == "resource"
                            select r;
            var resourceList = resources.ToList();
            if (resourceList.Count == 0)
                return Task.FromResult((ScampResource)null);
            return Task.FromResult(resourceList[0]);
        }

        public Task<IEnumerable<ScampResourceGroup>> GetResources()
        {
            var resources = from r in Client.CreateDocumentQuery<ScampResource>(Collection.SelfLink)
                            where r.Type == "resource"
                            select r;
            var resourceList = resources.ToList();
            return Task.FromResult((IEnumerable<ScampResourceGroup>)resourceList);
        }

        public Task<IEnumerable<ScampResourceGroup>> GetResourcesByOwner(string userId)
        {
            //TODO: need to add "join" to get by owner relationship
            var resources = from r in Client.CreateDocumentQuery<ScampResourceGroup>(Collection.SelfLink)
                            select r;
            var resourceList = resources.ToList();
            return Task.FromResult((IEnumerable<ScampResourceGroup>)resourceList);
        }

        public async Task CreateResource(ScampResource resource)
        {
            var created = await Client.CreateDocumentAsync(Collection.SelfLink, resource);
        }

        public Task<IEnumerable<ScampResourceGroup>> GetResourcesByGroup(string userId)
        {
            //TODO: need to add "join" to get by group relationship
            var resources = from u in Client.CreateDocumentQuery<ScampResourceGroup>(Collection.SelfLink)
                            select u;
            var resourceList = resources.ToList();
            return Task.FromResult((IEnumerable<ScampResourceGroup>)resourceList);
        }

        public Task AddResource(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }

        public void AddOwner(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }

        public async Task UpdateResource(ScampResource resource)
        {
            //TODO Check
            var resources = from u in Client.CreateDocumentQuery(Collection.SelfLink)
                            where u.Id == resource.Id 
                            select u;
            
            await Client.ReplaceDocumentAsync( resources.First().SelfLink,  resource);
        }
    }
}
