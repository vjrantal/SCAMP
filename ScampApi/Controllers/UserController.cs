using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampApi.ViewModels;
using DocumentDbRepositories.Implementation;
using DocumentDbRepositories;
using System.Threading.Tasks;

using System.Net.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers.Controllers
{
    [Authorize]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly ISecurityHelper _securityHelper;
        private readonly ILinkHelper _linkHelper;
        private readonly ResourceRepository _resourceRepository;
        private readonly UserRepository _userRepository;

        public UserController(ILinkHelper linkHelper, ISecurityHelper securityHelper, ResourceRepository resourceRepository, UserRepository userRepository)
        {
            _linkHelper = linkHelper;
            _resourceRepository = resourceRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
        }

        // retrieves the current user
        // GET: api/user
        [HttpGet(Name = "User.CurrentUser")]
        public async Task<User> Get()
        {
            ScampUser tmpUser = null;

            // fetch user
            tmpUser = await _securityHelper.GetCurrentUser();

            //TODO: we're going to need this for authorizing requests, so we should probably cache it
            //return object for user...

            return new User
            {
                Id = tmpUser.Id,
                Name = tmpUser.Name,
                isSystemAdmin = tmpUser.isSystemAdmin,
                email = tmpUser.email
            };
        }

        [HttpGet("{userId}", Name = "Users.GetSingle")]
        public async Task<User> Get(string userId)
        {
            // fetch user
            ScampUser tmpUser = await _userRepository.GetUserbyId(userId);

            //TODO what if user isn't found?

            return new User
            {
                Id = tmpUser.Id,
                Name = tmpUser.Name,
                isSystemAdmin = tmpUser.isSystemAdmin,
                email = tmpUser.email
            };
        }


        // get a list of the user's resources
        // GET /api/user/{userid}/resources/
        [HttpGet("resources", Name = "User.GetResourcesForUser")]
        public async Task<List<ScampResourceReference>> GetResourcesforUser(string userid)
        {
            List<ScampResourceReference> resourceList = new List<ScampResourceReference>();
            ScampUser currentUser = await _securityHelper.GetCurrentUser();

            // ensure requestor has system admin permissions
            if (!currentUser.isSystemAdmin && currentUser.Id == userid)
                throw new AccessViolationException("Access Denied");

            //TODO: execute query
            //resourceList = await _resourceRepository.GetResources(currentUser.Resources);

            // return results
            return resourceList;
        }


        [HttpGet("byname/{searchparm}", Name = "Users.SearchByName")]
        public User GetByName(string searchparm)
        {
            throw new NotImplementedException();
        }


        // POST api/values
        [HttpPost("{userId}")]
        public void Post(int userId,[FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{userId}")]
        public void Put(int userId, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        [HttpDelete("{userId}")]
        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

		private UserSummary map(ScampUser docDbUSer)
		{
			return new UserSummary
			{
				UserId = docDbUSer.Id,
				Name = docDbUSer.Name
			};
		}
	}
}
