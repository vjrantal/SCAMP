using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Authorize]
    [Route("api/groups")]
    public class GroupsController : Controller
    {
        private readonly ILinkHelper _linkHelper;
        private readonly GroupRepository _groupRepository;
        private readonly UserRepository _userRepository;
        private readonly ISecurityHelper _securityHelper;

        public GroupsController(ILinkHelper linkHelper, ISecurityHelper securityHelper,  GroupRepository groupRepository, UserRepository userRepository)
        {
            _linkHelper = linkHelper;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
        }
        [HttpGet(Name = "Groups.GetAll")]
        public async Task<IEnumerable<GroupSummary>> Get()
        {
            //LINKED TO UI
            var groups = await _groupRepository.GetGroups(await _securityHelper.GetUserReference());
            return groups.Select(MapToSummary);
        }

        [HttpGet("{groupId}", Name = "Groups.GetSingle")]
        public async Task<Group> Get(string groupId)
        {
            var group = await _groupRepository.GetGroupWithResources(groupId);
            return Map(group);
        }

        [HttpPost]
        public void Post([FromBody]Group group)
        {
            // TODO implement adding a group
            throw new NotImplementedException();
        }

        [HttpPut("{groupId}")]
        public void Put(int groupId, [FromBody]Group value)
        {
            // TODO implement updating a group
            throw new NotImplementedException();
        }

        [HttpDelete("{groupId}")]
        public void Delete(int groupId)
        {
            // TODO implement deleting a group
            throw new NotImplementedException();
        }


        private GroupSummary MapToSummary(ScampResourceGroup docDbGroup)
        {
            return new GroupSummary
            {
                GroupId = docDbGroup.Id,
                Name = docDbGroup.Name,
                Links = { new Link { Rel = "group", Href = _linkHelper.Group(groupId: docDbGroup.Id) } }
            };
        }
        private Group Map(ScampResourceGroupWithResources docDbGroup)
        {
            return new Group
            {
                GroupId = docDbGroup.Id,
                Name = docDbGroup.Name,
                Templates = new List<GroupTemplateSummary>(), // TODO map these when the repo supports them
                Members = docDbGroup.Members?.Select(MapToSummary),
                Admins= docDbGroup.Admins?.Select(MapToSummary),
                Resources = docDbGroup.Resources?.Select(MapToSummary)
            };  
        }
        private UserSummary MapToSummary(ScampUserReference docDbUser)
        {
            return new UserSummary
            {
                UserId = docDbUser.Id,
                Name = docDbUser.Name,
                Links =
                {
                    new Link { Rel = "user", Href= _linkHelper.User(docDbUser.Id) }
                }
            };
        }
        private ScampResourceSummary MapToSummary(ScampResource docDbResource)
        {
            return new ScampResourceSummary
            {
                ResourceGroup = new ScampResourceGroupReference() {Id= docDbResource.ResourceGroup.Id},
                Id  = docDbResource.Id,
                Name = docDbResource.Name,
                Links =
                {
                    new Link {Rel = "resource", Href = _linkHelper.GroupResource(docDbResource.ResourceGroup.Id, docDbResource.Id) }
                }
            };
        }
    }
}
