using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using DocumentDbRepositories;

namespace DocumentDbRepositories.Implementation
{

    public class UserRepository : RepositoryBase
    {
		public async Task CreateUser(ScampUser newUser)
		{
			var created = await Client.CreateDocumentAsync(Collection.SelfLink, newUser);
		}

		public Task<ScampUser> GetUser(string userId)
        {
            var users = from u in Client.CreateDocumentQuery<ScampUser>(Collection.SelfLink)
                        where u.Id == userId
                        select u;
            var userList = users.ToList();
            if (userList.Count == 0)
                return Task.FromResult((ScampUser)null);
            return Task.FromResult(userList[0]);           
        }

        public Task<ScampUser> GetUserByIPID(string IPID)
        {
            var users = from u in Client.CreateDocumentQuery<ScampUser>(Collection.SelfLink)
                        where u.IPKey == IPID
                        select u;
            var userList = users.ToList();
            if (userList.Count == 0)
                return Task.FromResult((ScampUser)null);
            return Task.FromResult(userList[0]);
        }
    }
}
