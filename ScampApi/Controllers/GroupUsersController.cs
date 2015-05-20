using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using System.Threading.Tasks;
using DocumentDbRepositories;
using System.Linq;

namespace ScampApi.Controllers
{
    [Route("api/group/{groupId}")]
    public class GroupUsersController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityHelper _securityHelper;

        public GroupUsersController(ISecurityHelper securityHelper, IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
        }

        /// <summary>
        /// returns a view of a group's information
        /// </summary>
        /// <param name="groupId">Id of group to get list of users for</param>
        /// <returns></returns>
        [HttpGet("users")]
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

        [HttpGet("user/{userId}/resources")]
        public async Task<IActionResult> Get(string groupId, string userId)
        {
            //TODO: add in group admin/manager authorization check
            //if (!await CurrentUserCanViewGroup(group))
            //    return new HttpStatusCodeResult(403); // Forbidden
            //}

            // get group details
            var tmpUser = await _userRepository.GetUserbyId(userId);
            if (tmpUser == null) // group not found, return appropriately
                return HttpNotFound();

            ScampUserGroupMbrship tmpGroup = tmpUser.GroupMembership.First(g => g.Id == groupId);
            if (tmpGroup == null) // user not found in group, return appropriately
                return HttpNotFound();

            // build return view
            List<ScampResourceSummary> rtnView = new List<ScampResourceSummary>();

            foreach (ScampUserGroupResources resourceRef in tmpGroup.Resources)
            {
                ScampResourceSummary tmpSummary = new ScampResourceSummary()
                {
                    Id = resourceRef.Id,
                    Name = resourceRef.Name,
                    totUnitsUsed = new Random().NextDouble() * (2000 - 100) + 100
                    //TODO: get state from volatile store
                };
                rtnView.Add(tmpSummary);
            }

            return new ObjectResult(rtnView) { StatusCode = 200 };

        }
    }
}