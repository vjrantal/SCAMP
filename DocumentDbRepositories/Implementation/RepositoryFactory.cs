using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentDbRepositories.Implementation;
using Microsoft.Framework.ConfigurationModel;

namespace DocumentDbRepositories.Implementation
{
    public class RepositoryFactory
    {
        private readonly string _authKey;
        private readonly string _endpoint;
        private readonly string _databaseName;
        private readonly string _collectionName;
        private string _connectionMode;

        public RepositoryFactory(IConfiguration configuration)
        {
            _endpoint = configuration["DocDb:endpoint"];
            _authKey = configuration["DocDb:authkey"];
            _databaseName = configuration["DocDb:databaseName"];
            _collectionName = configuration["DocDb:collectionName"];
            _connectionMode = configuration["DocDb:ConnectionMode"];
        }
        public Task<UserRepository> GetUserRepositoryAsync()
        {
            return Get<UserRepository>();
        }
        public Task<GroupRepository> GetGroupRepositoryAsync()
        {
            return Get<GroupRepository>();
        }
        public Task<ResourceRepository> GetResourceRepositoryAsync()
        {
            return Get<ResourceRepository>();
        }
        public Task<SubscriptionRepository> GetSubscriptionRepositoryAsync()
        {
            return Get<SubscriptionRepository>();
        }

        private async Task<TRepo> Get<TRepo>() where TRepo : RepositoryBase, new()
        {
            var repo = new TRepo();
            await repo.InitializeDBConnection(_endpoint, _authKey, _databaseName, _collectionName, _connectionMode);
            return repo;
        }
    }
}
