using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers.Controllers
{
    [Route("api/user/resourcees/{resourceId}/")]
    public class UserResourcesController : Controller
    {

        // POST api/values
        [HttpPost("{actionname}")]
        public void Post(string resourceId, string actionname)
        {
        }

        // POST api/values
        [HttpPost("connect")]
        public string Get(string resourceId)
        {
            return "rdpconnection 1"
        }

    }
}
