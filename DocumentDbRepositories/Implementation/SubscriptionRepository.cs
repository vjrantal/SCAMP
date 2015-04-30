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

    internal class SubscriptionRepository : ISubscriptionRepository
    {
        DocDb docdb;

        public SubscriptionRepository(DocDb docdb)
        {
            this.docdb = docdb;
        }

        public async Task CreateSubscription(ScampSubscription newSubscription)
        {
            if (!(await docdb.IsInitialized))
                return;

            var created = await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, newSubscription);
        }

        public async Task<ScampSubscription> GetSubscription(string subscriptionId)
        {
            if (!(await docdb.IsInitialized))
                return null;

            var subscriptions = from u in docdb.Client.CreateDocumentQuery<ScampSubscription>(docdb.Collection.SelfLink)
                                where u.Id == subscriptionId
                                select u;
            var subList = subscriptions.ToList();
            if (subList.Count == 0)
                return null;

            return subList[0];
        }

        public async Task<List<ScampSubscription>> GetSubscriptions()
        {
            if (!(await docdb.IsInitialized))
                return null;

            var subscriptions = from u in docdb.Client.CreateDocumentQuery<ScampSubscription>(docdb.Collection.SelfLink)
                                select u;

            return subscriptions.ToList();
        }
    }
}
