using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampApi.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private ILinkHelper _linkHelper;

        public UsersController(ILinkHelper linkHelper)
        {
            _linkHelper = linkHelper;
        }
        [HttpGet(Name = "Users.GetAll")]
        public IEnumerable<UserSummary> Get()
        {
            return new[] {
                new UserSummary {
                    Id =1,
                    Name = "User1",
                    UserUrl = _linkHelper.Users(1)

                },
                 new UserSummary {
                    Id =2,
                    Name = "User2",
                    UserUrl = _linkHelper.Users(2)

                }
            };
        }

        [HttpGet("{userId}", Name = "Users.GetSingle")]
        public User Get(int userId)
        {


            return new User
            {
                Id = 1,
                Name = "User1",
                Groups = new[] { new GroupSummary { GroupId = "Id1", Name = "Group1", GroupUrl = _linkHelper.Group(groupId: 1) } },
                Resources = new[] { new GroupResourceSummary { GroupId = 1, ResourceId = 1, Name = "GroupResource1", GroupResourceUrl = _linkHelper.GroupResource(groupId: 1, resourceId: 1) } }
            };
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
    }
}
