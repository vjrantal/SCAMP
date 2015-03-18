using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampApi.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private ILinkHelper _linkHelper;

        public UserController(ILinkHelper linkHelper)
        {
            _linkHelper = linkHelper;
        }
        // GET: api/user
        [HttpGet(Name = "User.Current")]
        public User Get()
        {
            //List groups and resources for the current user
            return new User { Id = 1, Name = "User1",
                Groups = new[] { new GroupSummary {  GroupId = 1, Name = "Group1", GroupUrl =  _linkHelper.Group(groupId:1) } },
                Resources = new[] { new GroupResourceSummary {  GroupId = 1, ResourceId = 1, Name = "GroupResource1", GroupResourceUrl =  _linkHelper.GroupResource(groupId:1, resourceId:1) } },
            };
        }
    }
}
