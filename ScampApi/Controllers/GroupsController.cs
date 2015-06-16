using System;
using System.Collections.Generic;
using System.Linq;
using R = System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using ProvisioningLibrary;
using Microsoft.AspNet.Authorization;

namespace ScampApi.Controllers
{
    [Authorize]
    [Route("api/groups")]
    public class GroupsController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityHelper _securityHelper;
        private static IVolatileStorageController _volatileStorageController = null;

        public GroupsController(ISecurityHelper securityHelper, IGroupRepository groupRepository, IUserRepository userRepository, IVolatileStorageController volatileStorageController)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
            _volatileStorageController = volatileStorageController;
        }

        [HttpGet(Name = "Groups.GetAll")]
        public async Task<IEnumerable<GroupSummary>> Get()
        {
            IEnumerable<ScampResourceGroup> groups;
            //LINKED TO UI
            if (await _securityHelper.IsSysAdmin())
            {
                groups = await _groupRepository.GetGroups();
            }
            else
            {
                groups = await _groupRepository.GetGroupsByUser(await _securityHelper.GetUserReference());
            }
            return groups.Select(MapToSummary);
        }

        /// <summary>
        /// retrieves full details on a single group
        /// </summary>
        /// <param name="groupId">Id of the group to be retrieved</param>
        /// <returns></returns>
        [HttpGet("{groupId}", Name = "Groups.GetSingle")]
        public async Task<IActionResult> Get(string groupId)
        {
            var group = await _groupRepository.GetGroupWithResources(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }
            bool userCanViewGroup = await CurrentUserCanViewGroup(group);
            if (!userCanViewGroup)
            {
                return new HttpStatusCodeResult(403); // Forbidden
            }
            return new ObjectResult(Map(group)) { StatusCode = 200 };
        }

        /// <summary>
        /// create a new group
        /// </summary>
        /// <param name="userInputGroup">group object to be created</param>
        /// <returns></returns>
        // creates a new group
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Group userInputGroup)
        {
            // requestor is a group administration and can create groups
            if (!(await _securityHelper.IsGroupAdmin()))
                return new HttpStatusCodeResult(403); // Forbidden;

            //TODO: group parameter validation 

            // get current user
            var currentUser = await _securityHelper.GetCurrentUser();

            // build the resource group object
            var group = new ScampResourceGroup()
            {
                Id = Guid.NewGuid().ToString(),
                Name = R.Regex.Replace(userInputGroup.Name.ToLowerInvariant(), "[^a-zA-Z0-9]", ""),
                Description = userInputGroup.Description,
                Budget = new ScampGroupBudget()
                {
                    OwnerId = currentUser.Id,
                    unitsBudgeted = userInputGroup.unitsBudgeted,
                    DefaultUserAllocation = userInputGroup.defaultUserBudget,
                    EndDate = userInputGroup.expiryDate
                }
            };

            await _groupRepository.CreateGroup(group);
            var resp = new GroupSummary()
            {
                Id = group.Id,
                Name = group.Name
            };

            return new ObjectResult(resp) { StatusCode = 200 };
        }

        [HttpPut("{groupId}")]
        public async Task<Group> Put(string groupId, [FromBody]Group value)
        {
            if (await _securityHelper.IsGroupManager(groupId))
            {
                //// we may need this
                //value.Admins.GroupBy(x => x.UserId).Select(y => y.First());	// remove duplicates
                //value.Members.GroupBy(x => x.UserId).Select(y => y.First());	// remove duplicates

                await _groupRepository.UpdateGroup(groupId, new ScampResourceGroup
                {
                    Members = value.Users.ConvertAll((a => new ScampUserGroupMbrship()
                    {
                        Id = a.Id,
                        Name = a.Name
                    })),
                    Id = value.Id,
                    Name = value.Name
                });

                return value;

            }
            return null;
        }

        private async Task<bool> CurrentUserCanViewGroup(ScampResourceGroupWithResources group)
        {
            var currentUser = await _securityHelper.GetCurrentUser();
            return currentUser.IsSystemAdmin                       // sys admin
                || group.Members.Any(u => u.Id == currentUser.Id && u.isAdmin); // group member
        }

        #region Mapping Functions
        private GroupSummary MapToSummary(ScampResourceGroup docDbGroup)
        {
            return new GroupSummary
            {
                Id = docDbGroup.Id,
                Name = docDbGroup.Name,
            };
        }

        private Group Map(ScampResourceGroupWithResources docDbGroup)
        {
            return new Group
            {
                Id = docDbGroup.Id,
                Name = docDbGroup.Name,
                Templates = new List<GroupTemplateSummary>(), // TODO map these when the repo supports them
                Users = docDbGroup.Members?.Select(MapToSummary).ToList(),
                unitsBudgeted = docDbGroup.Budget.unitsBudgeted,
                defaultUserBudget = docDbGroup.Budget.DefaultUserAllocation,
                expiryDate = docDbGroup.Budget.EndDate
            };
        }
        private UserSummary MapToSummary(ScampUserGroupMbrship docDbUser)
        {
            return new UserSummary
            {
                Id = docDbUser.Id,
                Name = docDbUser.Name,
                isAdmin = docDbUser.isAdmin
            };
        }

        #endregion
    }
}
