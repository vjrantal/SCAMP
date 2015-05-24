using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupId}/resources/{resourceId}/users")]
    public class GroupResourceUsersController : Controller
    {
        private ILinkHelper _linkHelper;

        public GroupResourceUsersController(ILinkHelper linkHelper)
        {
            _linkHelper = linkHelper;
        }

        [HttpPost]
        public void Post(string groupId, string resourceId, [FromBody]string userId)
        {
            // TODO implement adding a user to a group resource
            throw new NotImplementedException();
        }

        [HttpDelete("{userId}")]
        public void Delete(string groupId, string resourceId, string userId)
        {
            // TODO implement removing a user from a group resource
            throw new NotImplementedException();
        }
    }
}