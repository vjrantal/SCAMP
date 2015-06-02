using DocumentDbRepositories;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using ProvisioningLibrary;

namespace ScampApi.Controllers.Controllers
{
    //[Authorize]
    [Route("api/settings/subscriptions")]
    public class SettingsSubscriptionsController : Controller
    {
        private readonly ISystemSettingsRepository _settingsRepository;
        private ISecurityHelper _securityHelper;
        private static IVolatileStorageController _volatileStorageController;

        public SettingsSubscriptionsController(ISystemSettingsRepository settingsRepository, ISecurityHelper securityHelper, IVolatileStorageController volatileStorageController)
        {
            _settingsRepository = settingsRepository;
            _securityHelper = securityHelper;
            _volatileStorageController = volatileStorageController;
        }

        /// <summary>
        /// Retrieve a list of subscriptions
        /// </summary>
        /// <returns>List of subscriptions</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // only system admins can access this functionality
            //if (!await _securityHelper.IsSysAdmin())
            //    return new HttpStatusCodeResult(403); // Forbidden

            List<SubscriptionView> rtnView = new List<SubscriptionView>();

            // get group managers
            List<DocumentDbRepositories.ScampSubscription> subList = await _settingsRepository.GetSubscriptions();

            // build the return set
            foreach(DocumentDbRepositories.ScampSubscription sub in subList)
            {

                // build summary view object
                SubscriptionView newSubscription = new SubscriptionView()
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    AzureSubscriptionId = sub.AzureSubscriptionID,
                    AzureAdminUser = sub.AzureAdminUser
                };
                rtnView.Add(newSubscription);  // add it to the collection
            }

            // return list
            return new ObjectResult(rtnView) { StatusCode = 200 };
        }

        /// <summary>
        /// Retrieve a list of subscriptions
        /// </summary>
        /// <returns>List of subscriptions</returns>
        [HttpPut]
        public async Task<IActionResult> Upsert([FromBody] SubscriptionView subscription)
        {
            // only system admins can access this functionality
            if (!await _securityHelper.IsSysAdmin())
                return new HttpStatusCodeResult(403); // Forbidden

            ScampSubscription tmpSub = new ScampSubscription();

            //TODO: validate request
            if (!string.IsNullOrEmpty(subscription.Id)) // if doing update, subscription must already exist
            {
                tmpSub = await _settingsRepository.GetSubscription(subscription.Id);
                if (tmpSub == null)
                    return new ObjectResult("specified subscription does not exist") { StatusCode = 400 };
            }

            // map request to database object
            tmpSub.Name = subscription.Name;
            tmpSub.AzureSubscriptionID = subscription.AzureSubscriptionId;
            tmpSub.AzureAdminUser = subscription.AzureAdminUser;

            // if no subscription Id, do add
            if (string.IsNullOrEmpty(subscription.Id))
            {
                //tmpSub.AzureAdminPassword = subscription.AzureAdminPassword;
            }
            else // else, doing update
            {
                //if (!string.IsNullOrEmpty(subscription.AzureAdminPassword)) // if a password was specified
                //    tmpSub.AzureAdminPassword = subscription.AzureAdminPassword; // update it
            }

            // save insert/update subscription
            await _settingsRepository.UpsertSubscription(tmpSub);

            // return list
            return new ObjectResult(null) { StatusCode = 200 };
        }

        /// <summary>
        /// delete a subscription
        /// </summary>
        /// <returns>List of subscriptions</returns>
        [HttpDelete("{subscriptionId}")]
        public async Task<IActionResult> Delete(string subscriptionId)
        {
            // only system admins can access this functionality
            if (!await _securityHelper.IsSysAdmin())
                return new HttpStatusCodeResult(403); // Forbidden

            // get subscription
            ScampSubscription subscription = await _settingsRepository.GetSubscription(subscriptionId);
            if (subscription == null) // if not found
                return new ObjectResult("specified subscription does not exist") { StatusCode = 400 };

            //TODO: kick off process to clean up all resources in the subscription

            // mark subscription as deleted
            subscription.Deleted = true;
            await _settingsRepository.UpsertSubscription(subscription);

            // return success
            return new ObjectResult(null) { StatusCode = 204 };
        }

    }

}
