using System;
using System.Collections.Generic;
using System.Linq;
using ProvisioningLibrary;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers.Controllers
{
    [Route("api/user/resources/{resourceId}/")]
    public class UserResourcesController : Controller
    {


        IDictionary<string, string> settings = new Dictionary<string, string>();
        WebJobController _webJobController;

        public UserResourcesController(IConfiguration config)
        {

            var storageCstr = config.Get("Provisioning:StorageConnectionString");
            settings.Add("Provisioning:StorageConnectionString", storageCstr);

            _webJobController = new WebJobController(settings);
        }


        // POST /api/user/resources/{resourceId}/{actionname}
        [HttpPost("{actionname}")]
        public void Post(string  resourceId, string actionname)
        {
            _webJobController.SubmitActionInQueue(resourceId, actionname);
        }

        // GET /api/user/resources/{resourceId}/connect
        [HttpGet("connect")]
        public string Get(int resourceId)
        {
            //return the RDP connection string
            return "rdpconnection 1";
        }

    }
}
