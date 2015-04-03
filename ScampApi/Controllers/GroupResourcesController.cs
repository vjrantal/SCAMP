using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupId}/resources")]
    public class GroupResourcesController : Controller
    {
        private ILinkHelper _linkHelper;
        private ResourceRepository _resourceRepository;
        private ISecurityHelper _securityHelper;

        public GroupResourcesController(ILinkHelper linkHelper,ISecurityHelper securityHelper, ResourceRepository resourceRepository)
        {
            _linkHelper = linkHelper;
            _resourceRepository = resourceRepository;
            _securityHelper = securityHelper;
        }
        [HttpGet]
        public async Task< IEnumerable<ScampResourceSummary>> GetAll(string groupId)
        {
            //LINKED TO UI
            var res =await   _resourceRepository.GetResourcesByGroup(await _securityHelper.GetUserReference(), groupId);

            var ressummary = res.Select(Mapper.Map<ScampResourceSummary>).ToList();
            var rnd = new Random();
            ressummary.ForEach(summary =>
            {
                summary.Links.Add(new Link
                {
                    Rel = "resource",
                    Href = _linkHelper.GroupResource(summary.ResourceGroup.Id, summary.Id)
                });
                summary.Remaining = rnd.Next(0, 100);
            });

            return ressummary;
 
        }

        [HttpGet("{resourceId}", Name ="GroupResources.GetSingle")]
        public GroupResource Get(string groupId, string resourceId)
        {
            return new GroupResource
            {
                GroupId = groupId,
                ResourceId = resourceId,
                Name = "GroupResource" + resourceId,
                Users = new[]
                {
                    new UserSummary { UserId = "1", Name = "User1", Links =
                        {
                            new Link {Rel="user", Href = _linkHelper.User(userId: "1") } ,
                            new Link {Rel="groupResourceUser", Href = _linkHelper.GroupResourceUser(groupId: groupId, resourceId:resourceId, userId: "1") }
                        }
                    }
                }
            };
        }

        [HttpPost]
        public void Post([FromBody]GroupResource groupResource)
        {
            // TODO implement adding a resource to a group
            throw new NotImplementedException();
        }

        [HttpPut("{resourceId}")]
        public void Put(int groupId, string resourceId, [FromBody]GroupResource value)
        {
            // TODO implement updating a group resource
            throw new NotImplementedException();
        }

        [HttpDelete("{resourceId}")]
        public void Delete(int groupId, string resourceId)
        {
            // TODO implement removing a resource from a group
            throw new NotImplementedException();
        }
    }
}