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
    [Authorize]
    [Route("api/settings/groupmanagers")]
    public class SettingsGroupManagersController : Controller
    {
        private readonly ISystemSettingsRepository _settingsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private ISecurityHelper _securityHelper;
        private static IVolatileStorageController _volatileStorageController;

        public SettingsGroupManagersController(ISystemSettingsRepository settingsRepository, IUserRepository userRepository, ISecurityHelper securityHelper, IGroupRepository groupRepository, IVolatileStorageController volatileStorageController)
        {
            _settingsRepository = settingsRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _securityHelper = securityHelper;
            _volatileStorageController = volatileStorageController;
        }

        /// <summary>
        /// Retrieve a list of group managers
        /// </summary>
        /// <returns>List of users with System Admin permission</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // only system admins can access this functionality
            if (!await _securityHelper.IsSysAdmin())
                return new HttpStatusCodeResult(403); // Forbidden

            List<GroupManagerSummary> rtnView = new List<GroupManagerSummary>();

            // get group managers
            List<ScampUser> mgrList = await _settingsRepository.GetGroupManagers();

            // build the return set
            foreach(ScampUser usr in mgrList)
            {

                // get groups user controls budget of
                var groupList = await _groupRepository.GetGroupsByBudgetOwner(usr.Id);

                // calculate total amount of user's budget that has been used across all groups
                long totUsed = 0; // initial total used of users allocated budget
                foreach (var group in groupList)
                {
                    // get group's budget state
                    var groupBudget = await _volatileStorageController.GetGroupBudgetState(group.Id);
                    if (groupBudget != null) // if we found budget state
                        totUsed += groupBudget.UnitsUsed;
                }

                // build summary view object
                GroupManagerSummary newManager = new GroupManagerSummary()
                {
                    Id = usr.Id,
                    Name = usr.Name,
                    unitsBudgeted = usr.budget.Amount,
                    endDate = usr.budget.EndDate,
                    totUnitsUsed = totUsed,
                    totUnitsAllocated = usr.budget.Allocated,
                    totGroups = groupList.Count 
                };
                rtnView.Add(newManager);  // add it to the collection
            }

            // return list
            return new ObjectResult(rtnView) { StatusCode = 200 };
        }
    }
 
}
