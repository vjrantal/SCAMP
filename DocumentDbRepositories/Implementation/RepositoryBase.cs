using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDbRepositories
{
    public abstract class RepositoryBase
    {
        private DocumentClient _client;
        private Database _database;
        private DocumentCollection _collection;
        private bool _initialized = false;

        public async Task InitializeDBConnection(string endpointUrl, string authKey, string databaseName, string collectionName, string connectionMode)
        {
            ConnectionPolicy policy = GetConnectionPolocy(connectionMode);
            _client = new DocumentClient(new Uri(endpointUrl), authKey, policy);
            _database = await GetOrCreateDatabaseAsync(_client, databaseName);

            //Get, or Create, the Document Collection
            _collection = await GetOrCreateCollectionAsync(_client, _database.SelfLink, collectionName);
            _initialized = true;
        }

        private static ConnectionPolicy GetConnectionPolocy(string connectionMode)
        {
            connectionMode = connectionMode.ToLower();
            if (connectionMode == "http")
            {
                return new ConnectionPolicy()
                {
                    ConnectionProtocol = Protocol.Https,
                    ConnectionMode = ConnectionMode.Gateway
                };
            }
            else if (connectionMode == "tcp")
            {
                return new ConnectionPolicy()
                {
                    ConnectionProtocol = Protocol.Tcp,
                    ConnectionMode = ConnectionMode.Direct
                };
            }
            else
            {
                throw new ArgumentOutOfRangeException("ConnectionMode setting must be 'http' or 'tcp'");
            }
        }

        private async Task<Database> GetOrCreateDatabaseAsync(DocumentClient client, string id)
        {
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == id).ToArray().FirstOrDefault();
            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = id });
            }
            return database;
        }

        private async Task<DocumentCollection> GetOrCreateCollectionAsync(DocumentClient client, string dbLink, string id)
        {
            DocumentCollection collection = client.CreateDocumentCollectionQuery(dbLink).Where(c => c.Id == id).ToArray().FirstOrDefault();
            if (collection == null)
            {
                collection = await client.CreateDocumentCollectionAsync(dbLink, new DocumentCollection { Id = id });
            }

            return collection;
        }

        protected DocumentClient Client
        {
            get
            {
                ThrowIfNotInitialized();
                return _client;
            }
        }
        protected DocumentCollection Collection
        {
            get
            {
                ThrowIfNotInitialized();
                return _collection;
            }
        }

        private void ThrowIfNotInitialized()
        {
            if (!_initialized)
                throw new InvalidOperationException("Repository must be initialized before accessing the Client property");
        }
    }
}
