using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupid}/resources/{resourceid}/users")]
    public class GroupResourceUsersController : Controller
    {
        [HttpGet]
        public IEnumerable<GroupResourceUser> Get(int groupId, int resourceId)
        {
            return new[] {
                new GroupResourceUser { GroupId = groupId, ResourceId = resourceId, UserId = 1 },
                new GroupResourceUser { GroupId = groupId, ResourceId = resourceId, UserId = 2 },
                };
        }

        [HttpPost]
        public void Post([FromBody]GroupResourceUser groupResourceUser)
        {
            // TODO implement adding a user to a group resource
            throw new NotImplementedException();
        }

        [HttpDelete("{userId}")]
        public void Delete(int userId)
        {
            // TODO implement removing a user from a group resource
            throw new NotImplementedException();
        }
    }
}