using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace DocumentDbRepositories.Implementation
{
    public class DocDb
    {
        public IServiceCollection Services { get; private set; }

        public IConfiguration Config { get; private set; }

        public DocDb(IServiceCollection services, IConfiguration config)
        {
            this.Services = services;
            this.Config = config;
            this.IsInitialized = Task.Factory.StartNew(async () => await InitAsync()).Unwrap<bool>();

            // Sharing instance of DocumentClient as per "SDK Usage Tip #1: Use a singleton DocumentDB client for the lifetime of your application":
            // http://azure.microsoft.com/blog/2015/01/20/performance-tips-for-azure-documentdb-part-1-2/
            this.Services.AddInstance(this);
            this.Services.AddTransient<IGroupRepository, GroupRepository>();
            this.Services.AddTransient<IResourceRepository, ResourceRepository>();
            this.Services.AddTransient<IUserRepository, UserRepository>();
            this.Services.AddTransient<ISystemSettingsRepository, SystemSettingsRepository>();
        }

        public Task<bool> IsInitialized { get; private set; }

        public DocumentClient Client { get; private set; }

        public DocumentCollection Collection { get; private set; }

        public Database Database { get; private set; }

        public async Task<bool> InitAsync()
        {
            try
            {
                InitClient();
                await InitDatabaseAsync();
                await InitCollectionAsync();
                return true;
            }
            catch { return false; }
        }

        void InitClient()
        {
            var endpoint = new Uri(this.Config["DocDb:endpoint"]);
            var authKey = this.Config["DocDb:authkey"];
            var connectionPolicy = GetConnectionPolicy();
            Client = new DocumentClient(endpoint, authKey, connectionPolicy);
        }

        ConnectionPolicy GetConnectionPolicy()
        {
            var connectionMode = this.Config["DocDb:ConnectionMode"].ToLower();
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

        async Task InitDatabaseAsync()
        {
            var id = this.Config["DocDb:databaseName"];
            this.Database = this.Client.CreateDatabaseQuery().Where(db => db.Id == id).ToArray().FirstOrDefault();
            if (this.Database == null)
                this.Database = await this.Client.CreateDatabaseAsync(new Database { Id = id });
        }

        async Task InitCollectionAsync()
        {
            var id = this.Config["DocDb:collectionName"];
            this.Collection = this.Client.CreateDocumentCollectionQuery(this.Database.SelfLink).Where(c => c.Id == id).ToArray().FirstOrDefault();
            if (this.Collection == null)
                this.Collection = await this.Client.CreateDocumentCollectionAsync(this.Database.SelfLink, new DocumentCollection { Id = id });
        }

    }
}