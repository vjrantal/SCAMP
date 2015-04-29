using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDbRepositories.Implementation
{

    public class GroupRepository
    {
        private readonly DocumentClient _client;
        private readonly DocumentCollection _collection;

        public GroupRepository(DocumentClient client, DocumentCollection collection)
        {
            _client = client;
            _collection = collection;
        }
        public async Task<ScampResourceGroup> GetGroup(string groupID)
        {
            var groupQuery = from g in _client.CreateDocumentQuery<ScampResourceGroup>(_collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            var group = await groupQuery.AsDocumentQuery().FirstOrDefaultAsync();

            return group;
        }

        public async Task<ScampResourceGroupWithResources> GetGroupWithResources(string groupID)
        {
            var groupQuery = from g in _client.CreateDocumentQuery<ScampResourceGroupWithResources>(_collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            var groupTask = groupQuery.AsDocumentQuery().FirstOrDefaultAsync();

            var resourcesQuery = from r in _client.CreateDocumentQuery<ScampResource>(_collection.SelfLink)
                                 where r.Type == "resource" && r.ResourceGroup.Id == groupID
                                 select r;

            var resourcesTask = resourcesQuery.AsDocumentQuery().ToListAsync();

            await Task.WhenAll(groupTask, resourcesTask);

            var group = groupTask.Result;
            if (group == null)
                return null;

            group.Resources = resourcesTask.Result;

            return group;
        }
        public async Task<IEnumerable<ScampResourceGroup>> GetGroups()
        {
            var groups =
                _client.CreateDocumentQuery<ScampResourceGroup>(_collection.SelfLink)
                    .Where(u => u.Type == "group");
            var grouplist = await groups.AsDocumentQuery().ToListAsync();
            return grouplist;
        }
        public async Task<IEnumerable<ScampResourceGroup>> GetGroupsByUser(ScampUserReference user)
        {
            // Want to be able to write this in LINQ (but get a runtime exception):
            //var groups =
            //    (from grp in _client.CreateDocumentQuery<ScampResourceGroup>(_collection.SelfLink)
            //    where grp.Type == "group"
            //    from admin in grp.Admins
            //    where admin.Id == user.Id
            //    select grp)
            //    .ToList();
            var groups = _client.CreateDocumentQuery<ScampResourceGroup>(
                _collection.SelfLink,
                new SqlQuerySpec
                {
                    QueryText = " SELECT VALUE g" +
                                " FROM groups g " +
                                " JOIN admin in g.admins" +
                                " WHERE g.type = 'group'" +
                                " AND admin.id = @adminId",
                    Parameters = new SqlParameterCollection
                    {
                       new SqlParameter { Name = "@adminId", Value = user.Id }
                    }
                }
            );
            return await groups.AsDocumentQuery().ToListAsync();
        }

        public async Task CreateGroup(ScampResourceGroup newGroup)
        {
            var created = await _client.CreateDocumentAsync(_collection.SelfLink, newGroup);
        }

        public Task AddResource(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }

        public async Task UpdateGroup(string groupID, ScampResourceGroup group)
        {
            // TODO: Security
            Document document = await _client.CreateDocumentQuery(_collection.SelfLink)
                .Where(d => d.Id == groupID)
                .AsDocumentQuery()
                .FirstOrDefaultAsync();
            await _client.ReplaceDocumentAsync(document.SelfLink, group);
        }
        public Task AddAdmin(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }
	}
}
