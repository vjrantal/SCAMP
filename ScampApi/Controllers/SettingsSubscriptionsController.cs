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

            List<SubscriptionSummary> rtnView = new List<SubscriptionSummary>();

            // get group managers
            List<ScampSubscription> subList = await _settingsRepository.GetSubscriptions();

            // build the return set
            foreach(ScampSubscription sub in subList)
            {

                // build summary view object
                SubscriptionSummary newSubscription = new SubscriptionSummary()
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    AzureSubscriptionId = sub.AzureSubscriptionID
                };
                rtnView.Add(newSubscription);  // add it to the collection
            }

            // return list
            return new ObjectResult(rtnView) { StatusCode = 200 };
        }
    }
 
}
