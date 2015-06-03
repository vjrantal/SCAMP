using DocumentDbRepositories;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;

namespace ScampApi.Controllers.Controllers
{
    [Authorize]
    [Route("api/settings/sysadmins")]
    public class SettingsSysAdminController : Controller
    {
        private readonly ISystemSettingsRepository _settingsRepository;
        private readonly IUserRepository _userRepository;
        private ISecurityHelper _securityHelper;

        public SettingsSysAdminController(ISystemSettingsRepository settingsRepository, IUserRepository userRepository, ISecurityHelper securityHelper)
        {
            _settingsRepository = settingsRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
        }

        /// <summary>
        /// Retrieve a list of system administrators
        /// </summary>
        /// <returns>List of users with System Admin permission</returns>
        [HttpGet(Name = "Admin.Get")]
        public async Task<IActionResult> Get()
        {
            // only system admins can access this functionality
            if (!await _securityHelper.IsSysAdmin())
                return new HttpStatusCodeResult(403); // Forbidden

            List<UserSummary> rtnView = new List<UserSummary>();

            // fetch user from database
            List<ScampUser> adminList = await _settingsRepository.GetSystemAdministrators();

            // map data model to view model
            if (adminList != null)
            {
                foreach (ScampUser tmpUser in adminList)
                {
                    rtnView.Add(new UserSummary
                    {
                        Id = tmpUser.Id,
                        Name = tmpUser.Name
                    });
                }
            }

            // return list
            return new ObjectResult(rtnView) { StatusCode = 200 };
        }

        // grant a user system administrator permission
        [HttpPost("{userId}", Name = "Admin.Grant")]
        public async Task<IActionResult> grantAdmin(string userId)
        {
            // grant specified user "system admin" rights
            return await updateAdmin(userId, true);
        }

        // revoke system administrator permissions for a user
        [HttpDelete("{userId}", Name = "Admin.Revoke")]
        public async Task<IActionResult> revokeAdmin(string userId)
        {
            // check for last remaining admin
            List<ScampUser> adminList = await _settingsRepository.GetSystemAdministrators();
            if (adminList.Count <= 1)
                return new ObjectResult("This would remove the last system admin. There must always be at least 1.") { StatusCode = 403 };

            // revoke admin rights
            return await updateAdmin(userId, false);
        }

        private async Task<IActionResult> updateAdmin(string userId, bool isAdmin)
        {
            // ensure requestor has system admin permissions
            if (!await _securityHelper.IsSysAdmin())
                return new ObjectResult("Access Denied, requestor is not a system administrator") { StatusCode = 403 };

            ScampUser user = await _userRepository.GetUserbyId(userId);
            
            // if user wasn't found, add them to the scamp DB
            if (user == null)
            {
                return new ObjectResult("specified user does not exist") { StatusCode = 400 };

            }

            // only perform update if value needs to be changed
            if (user.IsSystemAdmin != isAdmin)
            {
                // if not, grant them permission
                user.IsSystemAdmin = isAdmin;

                // save updated setting
                await _userRepository.UpdateUser(user);
            }

            return new ObjectResult(null) { StatusCode = 200 };
        }
    }
}
