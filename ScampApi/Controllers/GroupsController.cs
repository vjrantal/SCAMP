using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups")]
    public class GroupsController : Controller
    {
        private ILinkHelper _linkHelper;

        public GroupsController(ILinkHelper linkHelper)
        {
            _linkHelper = linkHelper;
        }
        [HttpGet(Name = "Groups.GetAll")]
        public IEnumerable<GroupSummary> Get()
        {
            return new[] {
                new GroupSummary { GroupId = 1, Name = "Group1", GroupUrl = _linkHelper.Group(groupId: 1) },
                new GroupSummary { GroupId = 2, Name = "Group2", GroupUrl = _linkHelper.Group(groupId: 2) },
                };
        }

        [HttpGet("{groupId}", Name = "Groups.GetSingle")]
        public Group Get(int groupId)
        {
            return new Group
            {
                GroupId = groupId,
                Name = "Group" + groupId,
                Resources = new[]
                {
                    new GroupResourceSummary { GroupId = groupId, ResourceId = 1, Name = "GroupResource1", GroupResourceUrl = _linkHelper.GroupResource(groupId: groupId, resourceId: 1) }
                },
                Templates= new[]
                {
                    new GroupTemplateSummary { GroupId = groupId, TemplateId = 1, Name = "GroupTemplate1", GroupTemplateUrl = _linkHelper.GroupTemplate(groupId: groupId, templateId: 1) }
                },
                Users = new[]
                {
                    new GroupUserSummary { GroupId = groupId, UserId = 1, Name = "User1", GroupUserUrl = _linkHelper.GroupUser(groupId: groupId, userId: 1) }
                }
            };
        }

        [HttpPost]
        public void Post([FromBody]Group group)
        {
            // TODO implement adding a group
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Group value)
        {
            // TODO implement updating a group
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // TODO implement deleting a group
            throw new NotImplementedException();
        }
    }
}
