using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using DocumentDbRepositories.Implementation;
using DocumentDbRepositories;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using ProvisioningLibrary;

namespace ScampApi.Controllers.Controllers
{
    //[Authorize]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly ISecurityHelper _securityHelper;
        private readonly ILinkHelper _linkHelper;
        private readonly IResourceRepository _resourceRepository;
        private readonly IUserRepository _userRepository;
        private static IVolatileStorageController _volatileStorageController = null;

        public UserController(ILinkHelper linkHelper, ISecurityHelper securityHelper, IResourceRepository resourceRepository, IUserRepository userRepository, IVolatileStorageController volatileStorageController)
        {
            _linkHelper = linkHelper;
            _resourceRepository = resourceRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
            _volatileStorageController = volatileStorageController;
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

            var user = new User()
            {
                Id = tmpUser.Id,
                Name = tmpUser.Name,
                Email = tmpUser.Email
            };

            return user;
        }

        /// <summary>
        /// Gets data on a specific user
        /// </summary>
        /// <param name="userId">Id of user being requested</param>
        /// <param name="view">type of view of the data to be returned</param>
        /// <returns>populated view object</returns>
        [HttpGet("{userId}/usage/{view}")]
        public async Task<IActionResult> Get(string userId, string view)
        {
            //TODO: authorization check

            // get requested user document
            ScampUser userDoc = await _userRepository.GetUserbyId(userId);
            if (userDoc == null)
                return HttpNotFound();

            if (view == "summary")
            {
                UserUsageSummary tmpUserSummary = new UserUsageSummary()
                {
                    totUnitsAllocated = new Random().NextDouble() * (2000 - 100) + 100,
                    unitsBudgeted = new Random().NextDouble() * (2000 - 100) + 100,
                    totUnitsUsed = new Random().NextDouble() * (2000 - 100) + 100,
                    totGroupMemberships = userDoc.GroupMembership.Count()
                };

                return new ObjectResult(tmpUserSummary) { StatusCode = 200 };
            }
            else
            {
                return new ObjectResult(string.Format("view '{0}' not supported", view)) { StatusCode = 400 };
            }

            return new ObjectResult(null) { StatusCode = 200 };

        }

        //// get a list of the user's resources
        //// GET /api/user/{userid}/resources/
        //[HttpGet("{userId}/resources", Name = "User.GetResourcesForUser")]
        //public async Task<List<ScampResourceSummary>> GetResourcesforUser(string userId)
        //{
        //    List<ScampResourceSummary> resourceList = new List<ScampResourceSummary>();
        //    ScampUser currentUser = await _securityHelper.GetCurrentUser();

        //    // request must be systemAdmin, or the requesting user
        //    if (!currentUser.IsSystemAdmin && currentUser.Id != userId)
        //        throw new AccessViolationException("Access Denied");

        //    // execute query
        //    ScampUser user = await _userRepository.GetUserbyId(userId);

        //    if (user != null)
        //    {
        //        foreach(ScampUserGroupMbrship groupMbrship in user.GroupMembership)
        //        {
        //            foreach(ScampUserGroupResources resource in groupMbrship.Resources)
        //            {
        //                ResourceState currentState = ResourceState.Unknown;
        //                // get resource state from volatile store
        //                // Brent - Try/Catch is temporary
        //                try
        //                {
        //                    currentState = await _volatileStorageController.GetResourceState(resource.Id);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine("Exception source: {0}", ex.Source);
        //                }

        //                ScampResourceSummary tmpSummary = new ScampResourceSummary()
        //                {
        //                    Id = resource.Id,
        //                    //ResourceGroup = new ScampResourceGroupReference()
        //                    //{
        //                    //    Id = groupMbrship.Id,
        //                    //    Name = groupMbrship.Name
        //                    //},
        //                    Name = resource.Name,
        //                    Type = resource.type,
        //                    State = currentState,
        //                    //TODO: replace with the REAL value
        //                    //Remaining = new Random().Next(0, 100)
        //                };

        //                resourceList.Add(tmpSummary);
        //            }
        //        }
        //    }
        //    //TODO: return "not found" 

        //    return resourceList;
        //}


        [HttpGet("byname/{searchparm}", Name = "Users.SearchByName")]
        public async Task<List<ScampUserReference>> GetByName(string searchparm)
        {
            //TODO: implement search
            string searchStr = "https://scamp.search.windows.net/indexes/userindex/docs?api-version=2015-02-28&search=brent";

            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(searchStr));
            var req = HttpWebRequest.CreateHttp(searchStr);
            req.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            req.Headers.Add("api-key:0228F8886E1F40C4B53F05316E5B9CA1");

            var response = (HttpWebResponse) await req.GetResponseAsync();
            var result = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

            var users = JsonConvert.DeserializeObject<List<ScampUserReference>>(result);
            return users;
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
				Id = docDbUSer.Id,
				Name = docDbUSer.Name
			};
		}
	}
}
