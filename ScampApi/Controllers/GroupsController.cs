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
        [HttpGet]
        public IEnumerable<GroupSummary> Get()
        {
            return new[] {
                new GroupSummary { GroupId = 1, Name = "Group1", GroupUrl = _linkHelper.Group(groupId: 1) },
                new GroupSummary { GroupId = 2, Name = "Group2", GroupUrl = _linkHelper.Group(groupId: 2) },
                };
        }

        [HttpGet("{id}")]
        public Group Get(int id)
        {
            return new Group { GroupId = id, Name = "Group" + id };
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
