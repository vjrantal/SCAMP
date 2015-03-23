using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers
{
    [Route("sampledata")]
    public class SampleDataController : Controller
    {
        private readonly RepositoryFactory _repositoryFactory;

        public SampleDataController(RepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> Index_Post()
        {
            var subscriptionId= Request.Form["subscriptionId"];
            var subscriptionMngCert = Request.Form["subscriptionMngCert"];
            
            if(Request.Form["addSampleData"]== "on")
            { 
                var groupRepo = await _repositoryFactory.GetGroupRepositoryAsync();
                await groupRepo.CreateGroup(new ScampResourceGroup
                {
                    Id = Guid.NewGuid().ToString("d"),
                    Name = "Classrome 1 (SampleData)",
                    Resources = new List<ScampResource>
                    {
                        new ScampResource
                        {
                            Id = Guid.NewGuid().ToString("d"),
                            Name = "Wordpress virtual machine (SampleData)",
                            ResourceType = "Virtual Machine",
                            State = "Not provisioned"
                        }
                    }
                });
            }
            if (Request.Form["addSubscription"] == "on")
            {
                var subRepo = await _repositoryFactory.GetSubscriptionRepositoryAsync();
                    await subRepo.CreateSubscription(new ScampSubscription
                    {
                        Id = Guid.NewGuid().ToString("d"),
                        AzureSubscriptionID = subscriptionId,
                        AzureManagementThumbnail = subscriptionMngCert
                    });
            }
            return Content("Done!");
        }
    }
}
