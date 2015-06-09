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

            var query = from g in docdb.Client.CreateDocumentQuery<ScampResourceGroup>(docdb.Collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            return await query.AsDocumentQuery().FirstOrDefaultAsync();
        }

        public async Task<ScampResourceGroupWithResources> GetGroupWithResources(string groupID)
        {
            if (!(await docdb.IsInitialized))
                return null;

            var groupQuery = from g in docdb.Client.CreateDocumentQuery<ScampResourceGroupWithResources>(docdb.Collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            var groupTask = groupQuery.AsDocumentQuery().FirstOrDefaultAsync();

            var resourcesQuery = from r in docdb.Client.CreateDocumentQuery<ScampResource>(docdb.Collection.SelfLink)
                                 where r.Type == "resource" && r.ResourceGroup.Id == groupID
                                 select r;
            var resourcesTask = resourcesQuery.AsDocumentQuery().ToListAsync();

            await Task.WhenAll(groupTask, resourcesTask);

            var group = groupTask.Result;
            if (group == null)
                return null;

            //group.Resources = resourcesTask.Result;
            return group;
        }

        public async Task<List<ScampResourceGroupWithResources>> GetGroupsByBudgetOwner(string userId)
        {
            if (!(await docdb.IsInitialized))
                return null;

            var groupQuery = from g in docdb.Client.CreateDocumentQuery<ScampResourceGroupWithResources>(docdb.Collection.SelfLink)
                             where g.Budget.OwnerId == userId && g.Type == "group"
                             select g;
            List<ScampResourceGroupWithResources> groupList = await groupQuery.AsDocumentQuery().ToListAsync();

            return groupList;
        }


        public async Task<IEnumerable<ScampResourceGroup>> GetGroups()
        {
            if (!(await docdb.IsInitialized))
                return null;

            var query =
                docdb.Client.CreateDocumentQuery<ScampResourceGroup>(docdb.Collection.SelfLink)
                    .Where(u => u.Type == "group");
            return await query.AsDocumentQuery().ToListAsync();
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
            var sql = new SqlQuerySpec
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
            };
            var query = docdb.Client.CreateDocumentQuery<ScampResourceGroup>(docdb.Collection.SelfLink, sql);
            return await query.AsDocumentQuery().ToListAsync();
        }

        public async Task CreateGroup(ScampResourceGroup newGroup)
        {
            if (!(await docdb.IsInitialized))
                return;

            await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, newGroup);
        }

        public async Task AddUserToGroup(string groupId, string userId)
        {
            if (!(await docdb.IsInitialized))
                return;

            StoredProcedure sproc = docdb.Client.CreateStoredProcedureQuery(docdb.Collection.SelfLink)
                .Where(s => s.Id == "AddUserToGroup").AsEnumerable().FirstOrDefault();

            await docdb.Client.ExecuteStoredProcedureAsync<dynamic>(sproc.SelfLink, groupId, userId);

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
            var query = docdb.Client.CreateDocumentQuery(docdb.Collection.SelfLink)
                .Where(d => d.Id == groupID);
            var document = await query.AsDocumentQuery().FirstOrDefaultAsync();
			await docdb.Client.ReplaceDocumentAsync(document.SelfLink, group);
        }

        public Task AddAdmin(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }
	}
}
