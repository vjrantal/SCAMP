using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupId}/resources")]
    public class GroupResourcesController : Controller
    {
        [HttpGet]
        public IEnumerable<GroupResourceSummary> GetAll(int groupId)
        {
            return new[] {
                new GroupResourceSummary { GroupId = groupId, ResourceId = 1, Name = "GroupResource1" },
                new GroupResourceSummary { GroupId = groupId, ResourceId = 2, Name = "GroupResource2" },
                };
        }

        [HttpGet("{resourceId}", Name ="GroupResources.GetSingle")]
        public GroupResource Get(int groupId, int resourceId)
        {
            return new GroupResource { GroupId = groupId, ResourceId = resourceId, Name = "GroupResource" + resourceId };
        }

        [HttpPost]
        public void Post([FromBody]GroupResource groupResource)
        {
            // TODO implement adding a resource to a group
            throw new NotImplementedException();
        }

        [HttpPut("{resourceId}")]
        public void Put(int groupId, int resourceId, [FromBody]GroupResource value)
        {
            // TODO implement updating a group resource
            throw new NotImplementedException();
        }

        [HttpDelete("{resourceId}")]
        public void Delete(int groupId, int resourceId)
        {
            // TODO implement removing a resource from a group
            throw new NotImplementedException();
        }
    }
}