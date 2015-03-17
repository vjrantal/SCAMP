using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ScampApi.ViewModels;

namespace ScampApi.Controllers
{
    [Route("api/[controller]")]
    public class GroupsController : Controller
    {
        [HttpGet]
        public IEnumerable<GroupSummary> Get()
        {
            return new[] {
                new GroupSummary { Id = 1, Name = "Group1" },
                new GroupSummary { Id = 2, Name = "Group2" },
                };
        }

        [HttpGet("{id}")]
        public Group Get(int id)
        {
            return new Group { Id = id, Name = "Group" + id };
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
