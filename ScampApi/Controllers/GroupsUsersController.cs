using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using System.Threading.Tasks;
using DocumentDbRepositories;
using System.Linq;
using ProvisioningLibrary;
using Microsoft.AspNet.Authorization;

namespace ScampApi.Controllers
{
    [Authorize]
    [Route("api/groups/{groupId}/users")]
    public class GroupsUsersController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityHelper _securityHelper;
        private static IVolatileStorageController _volatileStorageController = null;

        public GroupsUsersController(ISecurityHelper securityHelper, IGroupRepository groupRepository, IUserRepository userRepository, IVolatileStorageController volatileStorageController)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
            _volatileStorageController = volatileStorageController;
        }

        /// <summary>
        /// returns a view of a group's information
        /// </summary>
        /// <param name="groupId">Id of group to get list of users for</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string groupId)
        {
            //TODO: add in group admin/manager authorization check
            //if (!await CurrentUserCanViewGroup(group))
            //    return new HttpStatusCodeResult(403); // Forbidden
            //}

            // get group details
            var group = await _groupRepository.GetGroup(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }

            // build return view
            List<UserGroupSummary> rtnView = new List<UserGroupSummary>();

            foreach (ScampUserGroupMbrship userRef in group.Members)
            {
                // get user budget for this group
                var groupBudget = await _volatileStorageController.GetUserBudgetState(userRef.Id, group.Id);

                // build summary item for return
                UserGroupSummary tmpSummary = new UserGroupSummary()
                {
                    Id = userRef.Id,
                    Name = userRef.Name,
                    totUnitsUsed = groupBudget.UnitsUsed,
                    totUnitsRemaining = (groupBudget.UnitsBudgetted - groupBudget.UnitsUsed)
                };
                rtnView.Add(tmpSummary); // add item to list
            }

            // return list
            return new ObjectResult(rtnView) { StatusCode = 200 };

        }

        /// <summary>
        /// adds a user to a group
        /// </summary>
        /// <param name="groupId">Id of group to add user to</param>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddUserToGroup(string groupId, string userId)
        {
            //TODO: add in group admin/manager authorization check
            //if (!await CurrentUserCanViewGroup(group))
            //    return new HttpStatusCodeResult(403); // Forbidden
            //}

            // get group details
            var rscGroup = await _groupRepository.GetGroup(groupId);
            if (rscGroup == null)
            {
                return new ObjectResult("designated group does not exist") { StatusCode = 400 };
            }

            // make sure user isn't already in group
            IEnumerable<ScampUserGroupMbrship> userList = from ur in rscGroup.Members
                where ur.Id == userId
                select ur;
            if (userList.Count() > 0) // user is already in the list
                return new ObjectResult("designated user is already a member of specified group") { StatusCode = 400 };

            //TODO: Issue #152
            // check to make sure enough remains in the group allocation to allow add of user

            // create document updates
            await _groupRepository.AddUserToGroup(groupId, userId);

            // create volatile storage budget entry for user
            var newBudget = new UserBudgetState(userId, groupId)
            {
                UnitsBudgetted = rscGroup.Budget.DefaultUserAllocation,
                UnitsUsed = 0
            };
            await _volatileStorageController.AddUserBudgetState(newBudget);
            //TODO: Issue #152
            // update group budget allocation to reflect addition of new user


            // return list
            return new ObjectResult(null) { StatusCode = 200 };
        }

        /// <summary>
        /// updates a user within a group
        /// </summary>
        /// <param name="groupId">Id of group to within which to update user</param>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserInGroup(string groupId, [FromBody] UserSummary newUserSummary)
        {
            //TODO: add in group admin/manager authorization check
            //if (!await CurrentUserCanViewGroup(group))
            //    return new HttpStatusCodeResult(403); // Forbidden
            //}

            // get group details
            var rscGroup = await _groupRepository.GetGroup(groupId);
            if (rscGroup == null)
            {
                return new ObjectResult("designated group does not exist") { StatusCode = 400 };
            }

            // make sure user is in group
            IEnumerable<ScampUserGroupMbrship> userList = from ur in rscGroup.Members
                where ur.Id == newUserSummary.Id
                select ur;
            if (userList.Count() == 0) // user is not in the list
                return new ObjectResult("designated user is not in group") { StatusCode = 400 };

            //TODO: Issue #152
            // check to make sure enough remains in the group allocation to handle the new allocation

            // update document
            await _groupRepository.UpdateUserInGroup(groupId, newUserSummary.Id, newUserSummary.isAdmin);

            // update volatile storage budget entry for user
            await _volatileStorageController.UpdateUserBudgetAllocation(groupId, newUserSummary.Id, newUserSummary.budget.unitsBudgeted);

            return new ObjectResult(null) { StatusCode = 200 };
        }

        /// <summary>
        /// get the list of resources for the specified user and group
        /// </summary>
        /// <param name="groupId">group to check</param>
        /// <param name="userId">user to check</param>
        /// <returns>list of resources as a collection of ScampResourceSummary objects </returns>
        [HttpGet("{userId}/resources")]
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

            ScampUserGroupMbrship tmpGroup = tmpUser.GroupMembership.FirstOrDefault(g => g.Id == groupId);
            if (tmpGroup == null) // user not found in group, return appropriately
                return new HttpStatusCodeResult(204); // nothing found

            // build return view
            List<ScampResourceSummary> rtnView = new List<ScampResourceSummary>();

            foreach (ScampUserGroupResources resourceRef in tmpGroup.Resources)
            {
                // get resource usage
                var rscState = await _volatileStorageController.GetResourceState(resourceRef.Id);

                ScampResourceSummary tmpSummary = new ScampResourceSummary()
                {
                    Id = resourceRef.Id,
                    Name = resourceRef.Name,
                    State = rscState.State,
                    totUnitsUsed = rscState.UnitsUsed
                };
                rtnView.Add(tmpSummary);
            }

            return new ObjectResult(rtnView) { StatusCode = 200 };

        }
    }
}