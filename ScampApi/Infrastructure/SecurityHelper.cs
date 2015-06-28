using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DocumentDbRepositories;
using DocumentDbRepositories.Implementation;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using ProvisioningLibrary;

namespace ScampApi.Infrastructure
{
    
    public class SecurityHelper : ISecurityHelper
    {
        private readonly HttpContext Context;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private ScampUser cachedCurrentUser;

        public SecurityHelper(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IGroupRepository groupRepository)
        {
            Context = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public async Task<ScampUserReference> GetUserReference()
        {
            return  Mapper.Map<ScampUserReference>(await GetCurrentUser());
        } 

        // retrieves the current user
        public async Task<ScampUser> GetCurrentUser()
        {
            // get current user's Id from security context
            var userId = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier")).Value;

            // if user object isn't already in object, save to reduced DB hits
            if (cachedCurrentUser == null || cachedCurrentUser.Id != userId)
                cachedCurrentUser = await GetUserById(userId);
            
            // return object to call
            return cachedCurrentUser;
        }

        /// <summary>
        /// gets a specific user by their Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ScampUser> GetUserById(string userId)
        {
            // if user object is already in object, and matched requested value, return it
            if (cachedCurrentUser != null && cachedCurrentUser.Id == userId)
                return cachedCurrentUser;

            // user wasn't already cached, to get it
            ScampUser tmpUser = await _userRepository.GetUserbyId(userId);
            if (tmpUser == null) // insert if user doesn't exist
            {
                // build user object
                tmpUser = new ScampUser()
                {
                    Id = userId,
                    Name =
                        string.Format("{0} {1}", Context.User.FindFirst(ClaimTypes.GivenName).Value,
                            Context.User.FindFirst(ClaimTypes.Surname).Value).Trim(),
                    IsSystemAdmin = false,
                    // get email address
                    Email = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("email") || c.Type.Contains("upn")).Value
                };

                // insert into database   
                await _userRepository.CreateUser(tmpUser);
            }

            return tmpUser;
        }

        /// <summary>
        /// checks to see if the user is a group manager of any group they are a member of
        /// </summary>
         /// <returns>true if the user is</returns>
        public async Task<bool> IsGroupManager()
        {
            var user = await GetCurrentUser();
            // user is a manager of this group if they are in the group membership list and are flagged "isManager"
            var checkMgr = user.GroupMembership.ToList().Any(q => q.isManager);
            return checkMgr;
        }

        /// <summary>
        /// checks to see if the user is a group manager
        /// </summary>
        /// <param name="groupId">Id of group to be checked</param>
        /// <returns>true if the user is</returns>
        public async Task<bool> IsGroupManager(string groupId)
        {
            var user = await GetCurrentUser();
            var grp = await _groupRepository.GetGroup(groupId);
            // user is a manager of this group if they are in the group membership list and are flagged "isManager"
            var checkMgr = grp.Members.ToList().Any(q => q.Id == user.Id && q.isManager);
            return checkMgr;
        }

        /// <summary>
        /// checks to see if the user is a group admin  of a specific group (aka the owner)
        /// </summary>
        /// <param name="groupId">Id of group to be checked</param>
        /// <returns>true is the user is</returns>
        public async Task<bool> IsGroupAdmin(string groupId)
        {
            var user = await GetCurrentUser();
            var grp = await _groupRepository.GetGroup(groupId);
            // user is a manager of this group if they are in the group membership list and are flagged "isManager"
            var checkMgr = (grp.Budget.OwnerId == user.Id);
            return checkMgr;
        }

        /// <summary>
        /// checks to see if the user is a group manager
        /// </summary>
        /// <returns>true is the user is</returns>
        public async Task<bool> IsGroupAdmin()
        {
            var user = await GetCurrentUser();
            // user is a group admin if they have a budget > 0
            var checkMgr = user.budget != null && user.budget.unitsBudgeted > 0; 
            return checkMgr;
        }

        public async Task<bool> IsSysAdmin()
        {
            ScampUser user = await GetCurrentUser();

            return user.IsSystemAdmin;
        }
    }
}