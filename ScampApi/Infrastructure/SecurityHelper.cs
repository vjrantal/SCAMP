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
        private readonly UserRepository _userRepository;
        public SecurityHelper(IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
        {
            Context = httpContextAccessor.Value;
            _userRepository = userRepository;
        }

        public async Task<ScampUserReference> GetUserReference()
        {
            return  Mapper.Map<ScampUserReference>(await GetUser());
        } 
        public async Task<ScampUser> GetUser()
        {
            //TODO ADD Some cache
            var IPID = GetIPIDByContext();
            return await GetUserByIPID(IPID);
        }

        public string GetIPIDByContext()
        {
            // get Tenant and Object ID claims
            string tenantID = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("tenantid")).Value;
            string objectID = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier")).Value;
            // create SCAMP UserID
            string IPID = string.Format("{0}:{1}", tenantID, objectID);
            return IPID;
        }
        public async Task<ScampUser> GetUserByIPID(string IPID)
        {
            var tmpUser = await _userRepository.GetUserByIPID(IPID);
            if (tmpUser == null) // insert if user doesn't exist
            {
                // build user object
                tmpUser = new ScampUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name =
                        string.Format("{0} {1}", Context.User.FindFirst(ClaimTypes.GivenName).Value,
                            Context.User.FindFirst(ClaimTypes.Surname).Value).Trim(),
                    email = Context.User.Claims.FirstOrDefault(c => c.Type.Contains("upn")).Value,
                    IPKey = IPID,
                    IsSystemAdmin = true // temporary value
                };
                // insert into database   
                await _userRepository.CreateUser(tmpUser);
            }
            return tmpUser;
        }
    }
}