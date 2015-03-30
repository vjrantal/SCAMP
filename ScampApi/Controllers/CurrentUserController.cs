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
    [Route("api/currentUser")]
    public class CurrentUserController : Controller
    {
        private ILinkHelper _linkHelper;
        private readonly UserRepository _userRepository;

        public CurrentUserController(ILinkHelper linkHelper, UserRepository userRepository)
        {
            _linkHelper = linkHelper;
            _userRepository = userRepository;
        }
        // GET: api/currentUser
        [HttpGet(Name = "User.CurrentUser")]
        public async Task<User> Get()
        {
            ScampUser tmpUser = null; 

            // get Tenant and Object ID claims
            string tenantID = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("tenantid")).Value;
            string objectID = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier")).Value;
            // create SCAMP UserID
            string IPID = string.Format("{0}:{1}", tenantID, objectID);

            // fetch user from database
            tmpUser = await _userRepository.GetUserByIPID(IPID);
            if (tmpUser == null) // insert if user doesn't exist
            {
                // build user object
                tmpUser = new ScampUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = string.Format("{0} {1}", Context.User.FindFirst(ClaimTypes.GivenName).Value, Context.User.FindFirst(ClaimTypes.Surname).Value).Trim(),
                    email = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("upn")).Value,
                    IPKey = IPID,
                    IsSystemAdmin = true // temporary value
                };
                // insert into database   
                await _userRepository.CreateUser(tmpUser);
            }

            //TODO: we're going to need this for authorizing requests, so we should probably cache it
            //return object for user...
            return new User
            {
                Id = tmpUser.Id,
                Name = tmpUser.Name,
                Groups = new[] { new GroupSummary { GroupId = "Id1", Name = "Group1", Links = { new Link { Rel = "group", Href = _linkHelper.Group(groupId: "Id1") } } } },
                Resources = new[] { new GroupResourceSummary { GroupId = "Id1", ResourceId = "1", Name = "GroupResource1", Links = { new Link { Rel = "groupResource", Href = _linkHelper.GroupResource(groupId: "Id1", resourceId: "1") } } } },
            };
        }
    }
}
