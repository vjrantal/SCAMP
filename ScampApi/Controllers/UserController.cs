using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.ViewModels;
using ScampApi.Infrastructure;


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
        [HttpGet]
        public User Get()
        {
           
            //List groups and resources for the current user
            return new User { Id = 1, Name = "User1", UserGroupDetail = new List<string> { _linkHelper.Group(1) },UserResourceDetail = new List<string> { }  };
        }

      

    }
}
