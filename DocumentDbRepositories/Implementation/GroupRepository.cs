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

    internal class GroupRepository : IGroupRepository
    {
        DocDb docdb;

        public GroupRepository(DocDb docdb)
        {
            this.docdb = docdb;
        }

        public async Task<ScampResourceGroup> GetGroup(string groupID)
        {
            if (!(await docdb.IsInitialized))
                return null;

            var groupQuery = from g in docdb.Client.CreateDocumentQuery<ScampResourceGroup>(docdb.Collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            var group = groupQuery.ToList().FirstOrDefault();
            return group;
        }

        public async Task<ScampResourceGroupWithResources> GetGroupWithResources(string groupID)
        {
            if (!(await docdb.IsInitialized))
                return null;

            var groupQuery = from g in docdb.Client.CreateDocumentQuery<ScampResourceGroupWithResources>(docdb.Collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            var group = groupQuery.ToList().FirstOrDefault();
            if (group == null)
                return null;

            var resourcesQuery = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
                                 where r.Type == "resource" && r.ResourceGroup.Id == groupID
                                 select r;
            group.Resources = resourcesQuery.ToList();
            return group;
        }

        public async Task<IEnumerable<ScampResourceGroup>> GetGroups()
        {
            if (!(await docdb.IsInitialized))
                return null;

            var groups =
                docdb.Client.CreateDocumentQuery<ScampResourceGroup>(docdb.Collection.SelfLink)
                    .Where(u => u.Type == "group");
            var grouplist = groups.ToList();
            return grouplist;
        }

        public async Task<IEnumerable<ScampResourceGroup>> GetGroupsByUser(ScampUserReference user)
        {
            if (!(await docdb.IsInitialized))
                return null;

            // Want to be able to write this in LINQ (but get a runtime exception):
            //var groups =
            //    (from grp in docdb.Client.CreateDocumentQuery<ScampResourceGroup>(docdb.Collection.SelfLink)
            //    where grp.Type == "group"
            //    from admin in grp.Admins
            //    where admin.Id == user.Id
            //    select grp)
            //    .ToList();
            var groups = docdb.Client.CreateDocumentQuery<ScampResourceGroup>(
                docdb.Collection.SelfLink,
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
            return groups;
        }

        public async Task CreateGroup(ScampResourceGroup newGroup)
        {
            if (!(await docdb.IsInitialized))
                return;

            var created = await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, newGroup);
        }

        public Task AddResource(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }

        public async Task UpdateGroup(string groupID, ScampResourceGroup group)
        {
            if (!(await docdb.IsInitialized))
                return;

            // TODO: Security
            Document document = docdb.Client.CreateDocumentQuery(docdb.Collection.SelfLink)
				.Where(d => d.Id == groupID)
				.AsEnumerable()
				.FirstOrDefault();
			await docdb.Client.ReplaceDocumentAsync(document.SelfLink, group);
        }

        public Task AddAdmin(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }
	}
}
