using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using DocumentDbRepositories;

namespace DocumentDbRepositories.Implementation
{

    public class GroupRepository
    {
        private DocumentClient client;
        private Database database;
        private DocumentCollection collection;

        public async Task InitializeDBConnection(string endpointUrl, string authKey, string databaseName, string collectionName)
        {
            var policy = new ConnectionPolicy()
            {
                ConnectionProtocol = Protocol.Tcp,
                ConnectionMode = ConnectionMode.Direct
            };
            client = new DocumentClient(new Uri(endpointUrl), authKey);//, policy);
            database = await GetOrCreateDatabaseAsync(databaseName);

            //Get, or Create, the Document Collection
            collection = await GetOrCreateCollectionAsync(database.SelfLink, collectionName);

        }

        public Task<ScampResourceGroup> GetGroup(string groupID)
        {
            // TODO - figure out why this mapping for Admins and Members 
            var groupQuery = from g in client.CreateDocumentQuery<ScampResourceGroup>(collection.SelfLink)
                             where g.Id == groupID
                             select g;
            var group = groupQuery.ToList().FirstOrDefault();
            //var groupQuery = client.CreateDocumentQuery<dynamic>(collection.SelfLink,
            //    new SqlQuerySpec(
            //        "SELECT * " +
            //        "FROM g " +
            //        "WHERE g.type='group' AND g.id = @groupid")
            //    {
            //        Parameters = new SqlParameterCollection
            //        {
            //            new SqlParameter("@groupid", groupID)
            //        }
            //    }
            //        );
            //var group = groupQuery.ToList().Select(g => new ScampResourceGroup
            //{
            //    Id = g.id,
            //    Name = g.name,
            //    Admins = ((IEnumerable<dynamic>)g.admins).Select(u =>
            //            new ScampUser
            //            {
            //                Id = u.id,
            //                Name =  u.name
            //            })
            //            .ToList(),
            //    Members = null // TODO
            //})
            //.FirstOrDefault();

            if (group == null)
                return Task.FromResult((ScampResourceGroup)null);

            var resourcesQuery = client.CreateDocumentQuery<dynamic>(collection.SelfLink,
                new SqlQuerySpec(
                    "SELECT * " +
                    "FROM r " +
                    "WHERE r.type='resource' AND r.parentgroup.id = @groupid")
                {
                    Parameters = new SqlParameterCollection
                    {
                        new SqlParameter("@groupid", groupID)
                    }
                }
                    );
            group.Resources = resourcesQuery.ToList()
                .Select(r => new ScampResource
                {
                    Id = r.id,
                    GroupId = r.resourcegroup.id,
                    Name = r.Name,
                    AzureResourceId = r.azureResourceId,
                    SubscriptionId = r.subscriptionId,
                    ResourceType = r.resourceType,
                    State = r.state,
                    Owners = ((IEnumerable<dynamic>)r.owners)
                                .Select(o=> new ScampUser
                                {
                                    Id = o.id,
                                    Name = o.name
                                })
                                .ToList()
                })
                .ToList();

            return Task.FromResult(group);

        }

        public Task<IEnumerable<ScampResourceGroup>> GetGroups()
        {
            var groups = from u in client.CreateDocumentQuery<ScampResourceGroup>(collection.SelfLink)
                         where u.type == "group"
                         select u;
            var grouplist = groups.ToList();
            return Task.FromResult((IEnumerable<ScampResourceGroup>)grouplist);

        }

        public async Task CreateGroup(ScampResourceGroup newGroup)
        {
            var created = await client.CreateDocumentAsync(collection.SelfLink, newGroup);
        }

        public Task AddResource(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }

        public Task AddMember(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }
        public Task AddAdmin(string groupID)
        {
            //TODO: stuff
            throw new NotImplementedException();
        }

        private async Task<Database> GetOrCreateDatabaseAsync(string id)
        {
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == id).ToArray().FirstOrDefault();
            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = id });
            }

            return database;
        }

        private async Task<DocumentCollection> GetOrCreateCollectionAsync(string dbLink, string id)
        {
            DocumentCollection collection = client.CreateDocumentCollectionQuery(dbLink).Where(c => c.Id == id).ToArray().FirstOrDefault();
            if (collection == null)
            {
                collection = await client.CreateDocumentCollectionAsync(dbLink, new DocumentCollection { Id = id });
            }

            return collection;
        }

    }
}
