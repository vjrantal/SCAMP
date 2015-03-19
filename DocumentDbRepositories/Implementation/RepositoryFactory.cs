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

        public RepositoryFactory(IConfiguration configuration)
        {
            _endpoint = configuration["DocDb:endpoint"];
            _authKey = configuration["DocDb:authkey"];
        }
        public async Task<UserRepository> GetUserRepositoryAsync()
        {
            var repo = new UserRepository();
            await repo.InitializeDBConnection(_endpoint, _authKey);
            return repo;
        }
        public async Task<GroupRepository> GetGroupRepositoryAsync()
        {
            var repo = new GroupRepository();
            await repo.InitializeDBConnection(_endpoint, _authKey);
            return repo;
        }
        public async Task<ResourceRepository> GetResourceRepositoryAsync()
        {
            var repo = new ResourceRepository();
            await repo.InitializeDBConnection(_endpoint, _authKey);
            return repo;
        }
        public async Task<SubscriptionRepository> GetSubscriptionRepositoryAsync()
        {
            var repo = new SubscriptionRepository();
            await repo.InitializeDBConnection(_endpoint, _authKey);
            return repo;
        }
    }
}
