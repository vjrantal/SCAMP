using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupId}/resources/{resourceId}/users")]
    public class GroupResourceUsersController : Controller
    {
        [HttpGet]
        public IEnumerable<GroupResourceUser> GetAll(int groupId, int resourceId)
        {
            return new[] {
                new GroupResourceUser { GroupId = groupId, ResourceId = resourceId, UserId = 1 },
                new GroupResourceUser { GroupId = groupId, ResourceId = resourceId, UserId = 2 },
                };
        }
        [HttpGet(Name ="GroupResourceUsers.GetSingle")]
        public GroupResourceUser Get(int groupId, int resourceId, int userId)
        {
            return new GroupResourceUser { GroupId = groupId, ResourceId = resourceId, UserId = userId };
        }

        [HttpPost]
        public void Post([FromBody]GroupResourceUser groupResourceUser)
        {
            // TODO implement adding a user to a group resource
            throw new NotImplementedException();
        }

        [HttpDelete("{userId}")]
        public void Delete(int groupId, int resourceId, int userId)
        {
            // TODO implement removing a user from a group resource
            throw new NotImplementedException();
        }
    }
}