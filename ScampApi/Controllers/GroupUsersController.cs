using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupId}/users")]
    public class GroupUsersController : Controller
    {
        [HttpGet]
        public IEnumerable<GroupUserSummary> Get(int groupId)
        {
            return new[] {
                new GroupUserSummary { GroupId = groupId, UserId = 1, Name = "GroupUser1" },
                new GroupUserSummary { GroupId = groupId, UserId = 2, Name = "GroupUser2" },
                };
        }

        [HttpGet("{userId}", Name="GroupUsers.GetSingle")]
        public GroupUser Get(int groupId, int userId)
        {
            return new GroupUser { GroupId = groupId, UserId = userId, Name = "GroupUser" + userId };
        }

        [HttpPost]
        public void Post([FromBody]GroupUser groupResource)
        {
            // TODO implement adding a user to a group
            throw new NotImplementedException();
        }

        [HttpPut("{userId}")]
        public void Put(int groupId, int userId, [FromBody]GroupUser value)
        {
            // TODO implement updating a group user
            throw new NotImplementedException();
        }

        [HttpDelete("{userId}")]
        public void Delete(int groupId, int userId)
        {
            // TODO implement removing a user from a group
            throw new NotImplementedException();
        }
    }
}