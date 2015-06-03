using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentDbRepositories.Implementation
{

    internal class SystemSettingsRepository : ISystemSettingsRepository
    {
        DocDb docdb;

        public SystemSettingsRepository(DocDb docdb)
        {
            this.docdb = docdb;
        }

        // get a list of system administrators
        public async Task<List<ScampUser>> GetSystemAdministrators()
        {
            if (!(await docdb.IsInitialized))
                return null;

            var admins = from u in docdb.Client.CreateDocumentQuery<ScampUser>(docdb.Collection.SelfLink)
                         where u.IsSystemAdmin == true
                         select u;
            var adminList = await admins.AsDocumentQuery().ToListAsync();
            return adminList;
        }

        // get a list of system administrators
        public async Task<List<ScampUser>> GetGroupManagers()
        {
            if (!(await docdb.IsInitialized))
                return null;

            var managers = from u in docdb.Client.CreateDocumentQuery<ScampUser>(docdb.Collection.SelfLink)
                         where u.budget != null && u.Type == "user"
                           select u;
            var managerList = await managers.AsDocumentQuery().ToListAsync();
            return managerList;
        }
        

        public async Task<StyleSettings> GetSiteStyleSettings()
        {
            string rtnResult = string.Empty;

            if (!(await docdb.IsInitialized))
                return null;

            // assumption is there is only one document of type "stylesettings"
            var settingQuery = from s in docdb.Client.CreateDocumentQuery<StyleSettings>(docdb.Collection.SelfLink)
                               where s.Type == "stylesettings"
                               select s;
            // execute query and return results
            return await settingQuery.AsDocumentQuery().FirstOrDefaultAsync(); ;
        }

        #region Subscription Management Methods
        public async Task UpsertSubscription(ScampSubscription subscription)
        {
            if (!(await docdb.IsInitialized))
                return;

            if (string.IsNullOrEmpty(subscription.Id))
            {
                subscription.Id = Guid.NewGuid().ToString();
                await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, subscription);
            }
            else
                await docdb.Client.ReplaceDocumentAsync(subscription.SelfLink, subscription);

            // exception handling, etc... 
        }

        /// <summary>
        /// Deletes a subscription document. Should only be used on failure of keyvault insertion
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns>nothing</returns>
        public async Task DeleteSubscription(ScampSubscription subscription)
        {
            if (!(await docdb.IsInitialized))
                throw new Exception("DocumentDB Controller did not initialize");

            await docdb.Client.DeleteDocumentAsync(subscription.SelfLink);
        }


        public async Task<ScampSubscription> GetSubscription(string subscriptionId)
        {
            if (!(await docdb.IsInitialized))
                return null;

            var query = from sub in docdb.Client.CreateDocumentQuery<ScampSubscription>(docdb.Collection.SelfLink)
                        where sub.Id == subscriptionId && sub.Deleted == false && sub.Type == "subscription"
                        select sub;
            return await query.AsDocumentQuery().FirstOrDefaultAsync();
        }

        public async Task<List<ScampSubscription>> GetSubscriptions()
        {
            if (!(await docdb.IsInitialized))
                return null;

            var query = from sub in docdb.Client.CreateDocumentQuery<ScampSubscription>(docdb.Collection.SelfLink)
                        where sub.Type == "subscription" && sub.Deleted == false
                        select sub;
            return await query.AsDocumentQuery().ToListAsync();
        }

        #endregion
    }
}
