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
    public class UserRepository
    {
        private readonly DocumentClient _client;
        private readonly DocumentCollection _collection;

        public UserRepository(DocumentClient client, DocumentCollection collection)
        {
            _client = client;
            _collection = collection;
        }
        public async Task CreateUser(ScampUser newUser)
		{
			var created = await _client.CreateDocumentAsync(_collection.SelfLink, newUser);
		}

		public async Task<ScampUser> GetUserbyId(string userId)
        {
            // get specified user by ID
            var queryResult = from u in _client.CreateDocumentQuery<ScampUser>(_collection.SelfLink)
                              where u.Id == userId
                              select u;
            var user = await queryResult.AsDocumentQuery().FirstOrDefaultAsync();

            return user;
        }

        public async Task UpdateUser(ScampUser user)
        {
            //TODO: likely need to do more here

            //ScampUser tmpUser = (dynamic)userDoc;
            user.isSystemAdmin = false;
            var savedUser = await _client.ReplaceDocumentAsync(user.SelfLink, user);

            // exception handling, etc... 

        }
    }
}
