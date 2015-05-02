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

namespace ScampApi.Infrastructure
{
    
    public class SecurityHelper : ISecurityHelper
    {
        private readonly HttpContext Context;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public SecurityHelper(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IGroupRepository groupRepository )
        {
            Context = httpContextAccessor.Value;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public async Task<ScampUserReference> GetUserReference()
        {
            return  Mapper.Map<ScampUserReference>(await GetCurrentUser());
        } 

        // retrieves the current user and stores it in cache for future calls
        public async Task<ScampUser> GetCurrentUser()
        {
            var userId = GetIPIDByContext();
            //TODO check if user is in cache
            ScampUser tmpUser = await GetUserById(userId);
            // TODO put the user object into cache
            return tmpUser;
        }

        public string GetIPIDByContext()
        {
            // get Tenant and Object ID claims
            string tenantID = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("tenantid")).Value;
            string objectID = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier")).Value;
            // create SCAMP UserID
            string IPID = string.Format("{0}-{1}", tenantID, objectID);
            return IPID;
        }

        public async Task<ScampUser> GetUserById(string userId)
        {
            var tmpUser = await _userRepository.GetUserbyId(userId);
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
        public async Task<bool> IsGroupAdmin(string groupId)
        {
            var user = await GetCurrentUser();
            var grp = await _groupRepository.GetGroup(groupId);
            var checkAdmin = grp.Admins.ToList().Any(q => q.Id == user.Id);
            return checkAdmin;
        }

        public async Task<bool> IsSysAdmin()
        {
            ScampUser user = await GetCurrentUser();

            return user.IsSystemAdmin;
        }
    }
}