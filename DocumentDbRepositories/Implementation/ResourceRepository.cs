using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDbRepositories.Implementation
{
	internal class ResourceRepository : IResourceRepository
	{
        DocDb docdb;

		public ResourceRepository(DocDb docdb)
		{
            this.docdb = docdb;
		}
		public async Task<ScampResource> GetResource(string resourceId)
		{
            if (!(await docdb.IsInitialized))
                return null;

			//TODO Check
			var resources = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
							where r.Id == resourceId && r.Type == "resource"
							select r;
			var resourceList = resources.ToList();
			if (resourceList.Count == 0)
				return null;

			return resourceList[0];
		}

		public async Task<IEnumerable<ScampResource>> GetResources()
		{
            if (!(await docdb.IsInitialized))
                return null;

            var resources = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
							where r.Type == "resource"
							select r;
			var resourceList = resources.ToList();
			return resourceList;
		}

        // given a list of resourceId's, return the resource references
        public async Task<List<ScampResourceReference>> GetResources(List<string> resourceIds)
        {
            if (!(await docdb.IsInitialized))
                return null;

            List<ScampResourceReference> rtnList = new List<ScampResourceReference>();

            //TODO: build and execute the query

            return rtnList;
        }

		public async Task<IEnumerable<ScampResource>> GetResourcesByOwner(string userId)
		{
            if (!(await docdb.IsInitialized))
                return null;

            //TODO: need to add "join" to get by owner relationship
            var resources = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
							select r;

            return resources.ToList();
		}

		public async Task CreateResource(ScampResource resource)
		{
            if (!(await docdb.IsInitialized))
                return;

            var created = await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, resource);
		}

		public async Task<IEnumerable<ScampResource>> GetResourcesByGroup(ScampUserReference user, string groupId)
		{
            if (!(await docdb.IsInitialized))
                return null;

            var resources = docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
    				.Where(u => u.Type == "resource" && u.ResourceGroup.Id == groupId);

            // if not admin add where clause
            if (!IsGroupAdmin(user, groupId))
				resources = resources.Where(u => u.Owners[0].Id == user.Id);

			var resourceList = resources.ToList();
			return resourceList;
		}

		private bool IsGroupAdmin(ScampUserReference user, string groupId)
		{
            // check all group's admin
			return docdb.Client.CreateDocumentQuery<dynamic>(docdb.Collection.SelfLink, new SqlQuerySpec
			{
				QueryText = "SELECT g FROM Groups g JOIN u IN g.admins WHERE g.id = @groupId AND g.type='group' AND u.id = @userId",
				Parameters = new SqlParameterCollection()
					{
						  new SqlParameter("@groupId", groupId),
						  new SqlParameter("@userId", user.Id)
					}
			}).ToList().Any();
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
            if (!(await docdb.IsInitialized))
                return;
            
            //TODO Check
            var dbRes = (from u in docdb.Client.CreateDocumentQuery(docdb.Collection.SelfLink)
						 where u.Id == resource.Id
						 select u).ToList().FirstOrDefault();

			await docdb.Client.ReplaceDocumentAsync(dbRes.SelfLink, resource);
		}

		public async Task DeleteResource(string resourceId)
		{
            if (!(await docdb.IsInitialized))
                return;

            var dbRes = (from u in docdb.Client.CreateDocumentQuery(docdb.Collection.SelfLink)
						 where u.Id == resourceId
						 select u).ToList().FirstOrDefault();
			await docdb.Client.DeleteDocumentAsync(dbRes.SelfLink);
		}
	}
}
