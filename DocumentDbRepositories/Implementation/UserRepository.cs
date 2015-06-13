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
    internal class UserRepository : IUserRepository
    {
        DocDb docdb;
        public UserRepository(DocDb docdb)
        {
            this.docdb = docdb;
        }
        public async Task CreateUser(ScampUser newUser)
		{
            if (!(await docdb.IsInitialized))
                return;

            var created = await docdb.Client.CreateDocumentAsync(docdb.Collection.SelfLink, newUser);
		}

		public async Task<ScampUser> GetUserbyId(string userId)
        {
            if (!(await docdb.IsInitialized))
                return null;

            // get specified user by ID
            var query = from u in docdb.Client.CreateDocumentQuery<ScampUser>(docdb.Collection.SelfLink)
                              where u.Id == userId
                              select u;
            return await query.AsDocumentQuery().FirstOrDefaultAsync();
        }

        public async Task UpdateUser(ScampUser user)
        {
            if (!(await docdb.IsInitialized))
                return;

            var savedUser = await docdb.Client.ReplaceDocumentAsync(user.SelfLink, user);

            //TODO: exception handling, etc... 
        }
    }
}
