using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers.Controllers
{
    [Route("api/user/resources/{resourceId}/")]
    public class UserResourcesController : Controller
    {

        // POST api/values
        [HttpPost("{actionname}")]
        public void Post(int resourceId, string actionname)
        {
        }

        // POST api/values
        [HttpGet("connect")]
        public string Get(int resourceId)
        {
            return "rdpconnection 1";
        }

    }
}
