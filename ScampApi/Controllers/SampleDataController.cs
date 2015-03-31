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
        private readonly GroupRepository _groupRepository;
        private readonly ResourceRepository _resourceRepository;
        private readonly SubscriptionRepository _subscriptionRepository;
        private readonly UserRepository _userRepository;

        public SampleDataController(
            UserRepository userRepository,
            GroupRepository groupRepository,
            ResourceRepository resourceRepository,
            SubscriptionRepository subscriptionRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _resourceRepository = resourceRepository;
            _subscriptionRepository = subscriptionRepository;
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
            var subscriptionId = Request.Form["subscriptionId"];
            var subscriptionMngCert = Request.Form["subscriptionMngCert"];

            if (Request.Form["addSampleData"] == "on")
            {
                var user1 = new ScampUser
                {
                    Id = Guid.NewGuid().ToString("d"),
                    Name = "Some User1"
                };
                var user2 = new ScampUser
                {
                    Id = Guid.NewGuid().ToString("d"),
                    Name = "Some User2"
                };
                await _userRepository.CreateUser(user1);
                await _userRepository.CreateUser(user2);

                var group = new ScampResourceGroup
                {
                    Id = Guid.NewGuid().ToString("d"),
                    Name = "Classroom Sample",
                    Admins = new List<ScampUserReference> { user1 },
                    Members = new List<ScampUserReference> { user1, user2 },
                };
                await _groupRepository.CreateGroup(group);

                await _resourceRepository.CreateResource(new ScampResource
                {
                    Id = Guid.NewGuid().ToString("d"),
                    ResourceGroup = new ScampResourceGroupReference { Id = group.Id },
                    Name = "ScampDev",
                    ResourceType = "Virtual Machine",
                    State = "Not provisioned"
                });
            }
            if (Request.Form["addSubscription"] == "on")
            {
                await _subscriptionRepository.CreateSubscription(new ScampSubscription
                {
                    Id = Guid.NewGuid().ToString("d"),
                    AzureSubscriptionID = subscriptionId,
                    AzureManagementThumbnail = subscriptionMngCert,

                });
            }
            return Content("Done!");
        }
    }
}
