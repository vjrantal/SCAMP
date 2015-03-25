using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

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
            var groupQuery = from g in client.CreateDocumentQuery<ScampResourceGroup>(collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            var group = groupQuery.ToList().FirstOrDefault();

            return Task.FromResult(group);
        }

        public Task<ScampResourceGroupWithResources> GetGroupWithResources(string groupID)
        {
            var groupQuery = from g in client.CreateDocumentQuery<ScampResourceGroupWithResources>(collection.SelfLink)
                             where g.Id == groupID && g.Type == "group"
                             select g;
            var group = groupQuery.ToList().FirstOrDefault();
            
            if (group == null)
                return Task.FromResult((ScampResourceGroupWithResources)null);

            var resourcesQuery = from r in client.CreateDocumentQuery<ScampResource>(collection.SelfLink)
                                 where r.Type == "resource" && r.ResourceGroup.Id == groupID
                                 select r;

            group.Resources = resourcesQuery.ToList();

            return Task.FromResult(group);
        }

        public Task<IEnumerable<ScampResourceGroup>> GetGroups()
        {
            var groups = from u in client.CreateDocumentQuery<ScampResourceGroup>(collection.SelfLink)
                         where u.Type == "group"
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
