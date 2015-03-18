using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupid}/templates")]
    public class GroupTemplatesController : Controller
    {
        [HttpGet]
        public IEnumerable<GroupTemplateSummary> Get(int groupId)
        {
            return new[] {
                new GroupTemplateSummary { GroupId = groupId, TemplateId = 1, Name = "GroupTemplate1" },
                new GroupTemplateSummary { GroupId = groupId, TemplateId = 2, Name = "GroupTemplate2" },
                };
        }

        [HttpGet("{templateid}", Name = "GroupTemplates.GetSingle")]
        public GroupTemplate Get(int groupId, int templateId)
        {
            return new GroupTemplate { GroupId = groupId, TemplateId = templateId, Name = "GroupTemplate" + templateId };
        }

        [HttpPost]
        public void Post([FromBody]GroupTemplate groupResource)
        {
            // TODO implement adding a template to a group
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]GroupTemplate value)
        {
            // TODO implement updating a group template
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // TODO implement removing a template from a group
            throw new NotImplementedException();
        }
    }
}