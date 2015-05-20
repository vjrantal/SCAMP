using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using System.Threading.Tasks;
using DocumentDbRepositories;

namespace ScampApi.Controllers
{
    [Route("api/groups/{groupId}/users")]
    public class GroupUsersController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ISecurityHelper _securityHelper;

        public GroupUsersController(ISecurityHelper securityHelper, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _securityHelper = securityHelper;
        }

        /// <summary>
        /// returns a view of a group's information
        /// </summary>
        /// <param name="groupId">Id of group to get list of users for</param>
        /// <param name="view">type of view to be returned</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string groupId)
        {
            //TODO: add in group admin/manager authorization check
            //if (!await CurrentUserCanViewGroup(group))
            //    return new HttpStatusCodeResult(403); // Forbidden
            //}

            // get group details
            var group = await _groupRepository.GetGroupWithResources(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }

            // build return view
            List<UserGroupSummary> rtnView = new List<UserGroupSummary>();

            foreach (ScampUserReference userRef in group.Members)
            {
                UserGroupSummary tmpSummary = new UserGroupSummary()
                {
                    Id = userRef.Id,
                    Name = userRef.Name,
                    totUnitsUsed = new Random().NextDouble() * (2000 - 100) + 100,
                    totUnitsRemaining = new Random().NextDouble() * (2000 - 100) + 100
                };
                rtnView.Add(tmpSummary);
            }

            return new ObjectResult(rtnView) { StatusCode = 200 };

        }
    }
}