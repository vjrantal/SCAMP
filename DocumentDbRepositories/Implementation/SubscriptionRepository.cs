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

            await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, newSubscription);
        }

        public async Task<ScampSubscription> GetSubscription(string subscriptionId)
        {
            if (!(await docdb.IsInitialized))
                return null;

            var query = from u in docdb.Client.CreateDocumentQuery<ScampSubscription>(docdb.Collection.SelfLink)
                                where u.Id == subscriptionId
                                select u;
            return await query.AsDocumentQuery().FirstOrDefaultAsync();
        }

        public async Task<List<ScampSubscription>> GetSubscriptions()
        {
            if (!(await docdb.IsInitialized))
                return null;

            var query = from u in docdb.Client.CreateDocumentQuery<ScampSubscription>(docdb.Collection.SelfLink)
                                select u;
            return await query.AsDocumentQuery().ToListAsync();
        }
    }
}
