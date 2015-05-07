using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampTypes.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupId}/templates")]
    public class GroupTemplatesController : Controller
    {
        [HttpGet]
        public IEnumerable<GroupTemplateSummary> Get(string groupId)
        {
            return new[] {
                new GroupTemplateSummary { Id = groupId, Name = "GroupTemplate1", TemplateId = "1" },
                new GroupTemplateSummary { Id = groupId, Name = "GroupTemplate2", TemplateId = "2" },
                };
        }

        [HttpGet("{templateId}", Name = "GroupTemplates.GetSingle")]
        public GroupTemplate Get(string groupId, string templateId)
        {
            return new GroupTemplate { Id = templateId, Name = ("GroupTemplate" + templateId), GroupId = groupId };
        }

        [HttpPost]
        public void Post([FromBody]GroupTemplate groupResource)
        {
            // TODO implement adding a template to a group
            throw new NotImplementedException();
        }

        [HttpPut("{templateId}")]
        public void Put(string groupId, int templateId, [FromBody]GroupTemplate value)
        {
            // TODO implement updating a group template
            throw new NotImplementedException();
        }

        [HttpDelete("{templateId}")]
        public void Delete(string groupId, int templateId)
        {
            // TODO implement removing a template from a group
            throw new NotImplementedException();
        }
    }
}