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

			var query = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
							where r.Id == resourceId && r.Type == "resource"
							select r;

            return await query.AsDocumentQuery().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<ScampResource>> GetResources()
		{
            if (!(await docdb.IsInitialized))
                return null;

            var query = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
							where r.Type == "resource"
							select r;

			return await query.AsDocumentQuery().ToListAsync();
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
            var query = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
							select r;

            return await query.AsDocumentQuery().ToListAsync();
        }

        public async Task CreateResource(ScampResource resource)
		{
            if (!(await docdb.IsInitialized))
                return;

            await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, resource);
		}

		public async Task<IEnumerable<ScampResource>> GetResourcesByGroup(ScampUserReference user, string groupId)
		{
            if (!(await docdb.IsInitialized))
                return null;

            var resources = docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
    				.Where(u => u.Type == "resource" && u.ResourceGroup.Id == groupId);

            // if not admin add where clause
            if (!await IsGroupAdminAsync(user, groupId))
				resources = resources.Where(u => u.Owners[0].Id == user.Id);

            return await resources.AsDocumentQuery().ToListAsync();
        }

		private async Task<bool> IsGroupAdminAsync(ScampUserReference user, string groupId)
		{
            // check all group's admin
            var sql = new SqlQuerySpec
            {
                QueryText = "SELECT g FROM Groups g JOIN u IN g.admins WHERE g.id = @groupId AND g.type='group' AND u.id = @userId",
                Parameters = new SqlParameterCollection()
                    {
                          new SqlParameter("@groupId", groupId),
                          new SqlParameter("@userId", user.Id)
                    }
            };
            var query = docdb.Client.CreateDocumentQuery<dynamic>(docdb.Collection.SelfLink, sql);
            var match = await query.AsDocumentQuery().FirstOrDefaultAsync();
            return match != null;
		}

		public async Task UpdateResource(ScampResource resource)
		{
            if (!(await docdb.IsInitialized))
                return;

            var query = from u in docdb.Client.CreateDocumentQuery(docdb.Collection.SelfLink)
                         where u.Id == resource.Id
                         select u;
            var document = await query.AsDocumentQuery().FirstOrDefaultAsync();
			await docdb.Client.ReplaceDocumentAsync(document.SelfLink, resource);
		}

		public async Task DeleteResource(string resourceId)
		{
            if (!(await docdb.IsInitialized))
                return;
            var query = from u in docdb.Client.CreateDocumentQuery(docdb.Collection.SelfLink)
                         where u.Id == resourceId
                         select u;
            var document = await query.AsDocumentQuery().FirstOrDefaultAsync();
			await docdb.Client.DeleteDocumentAsync(document.SelfLink);
		}
	}
}
