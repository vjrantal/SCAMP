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

    public class SubscriptionRepository : RepositoryBase
    {
        public async Task CreateSubscription(ScampSubscription newSubscription)
        {
            var created = await Client.CreateDocumentAsync(Collection.SelfLink, newSubscription);
        }

        public Task<ScampSubscription> GetSubscription(string subscriptionId)
        {
            var subscriptions = from u in Client.CreateDocumentQuery<ScampSubscription>(Collection.SelfLink)
                                where u.Id == subscriptionId
                                select u;
            var subList = subscriptions.ToList();
            if (subList.Count == 0)
                return Task.FromResult((ScampSubscription)null);
            return Task.FromResult(subList[0]);

        }

        public Task<List<ScampSubscription>> GetSubscriptions()
        {
            var subscriptions = from u in Client.CreateDocumentQuery<ScampSubscription>(Collection.SelfLink)
                                select u;
            var subList = subscriptions.ToList();
            if (subList.Count == 0)
                return Task.FromResult((List<ScampSubscription>)null);
            return Task.FromResult(subList);
        }
    }
}
