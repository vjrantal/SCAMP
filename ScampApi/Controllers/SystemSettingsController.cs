using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampApi.ViewModels;
using System.Security.Claims;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using System.Threading.Tasks;

namespace ScampApi.Controllers.Controllers
{
    [Authorize]
    [Route("api/settings/{actionname}")]
    public class SystemSettingsController : Controller
    {
        private ILinkHelper _linkHelper;
        private readonly SystemSettingsRepository _settingsRepository;
        private readonly UserRepository _userRepository;
        private ISecurityHelper _securityHelper;

        public SystemSettingsController(ILinkHelper linkHelper, SystemSettingsRepository settingsRepository, UserRepository userRepository)
        {
            _linkHelper = linkHelper;
            _settingsRepository = settingsRepository;
            _userRepository = userRepository;
        }

        // Retrieve a list of system administrators
        [HttpGet]
        [ActionName("admin")]
        public async Task<List<User>> Get()
        {
            List<User> rtnList = null;

            // fetch user from database
            List<ScampUser> adminList = await _settingsRepository.GetSystemAdministrators();

            // map data model to view model
            if (adminList != null)
            {
                rtnList = new List<User>();

                foreach (ScampUser tmpUser in adminList)
                {
                    rtnList.Add(new User
                    {
                        Id = tmpUser.Id,
                        Name = tmpUser.Name,
                        IsSystemAdmin = tmpUser.IsSystemAdmin
                    });
                }
            }        

            return rtnList;
        }

        // grant a user system administrator permission
        [HttpPost]
        [ActionName("admin")]
        public async void grantAdmin([FromBody]string userId)
        {
            // get user if it exists
            ScampUser user = await _userRepository.GetUser(userId);
            //TODO: make sure we got a user and throw a 404 if not 
            //if (user == null)
            //    throw new HttpResponseException(HttpStatusCode.NotFound);

            // is user already an admin?
            if (!user.IsSystemAdmin)
            {
                // if not, grant them permission
                user.IsSystemAdmin = true;

                // save updated setting
                user = await _userRepository.UpdateUser(user);
            }

        }
    }
}
