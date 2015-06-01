using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocDbSetup
{
    class Program
    {
        private static DocumentClient client;

        static void Main(string[] args)
        {
            // setup DocumentDB client 
            Uri endpoint = new Uri("");
            string authKey = "";
            string databaseId = "";
            string collectionId = "";

            var connectionPolicy = new ConnectionPolicy()
            {
                ConnectionProtocol = Protocol.Https,
                ConnectionMode = ConnectionMode.Gateway
            };
            client = new DocumentClient(endpoint, authKey, connectionPolicy);

            Database database = client.CreateDatabaseQuery().Where(db => db.Id == databaseId).ToArray().FirstOrDefault();
            DocumentCollection collection = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionId).ToArray().FirstOrDefault();

            // get list of SP files
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            foreach (FileInfo tmpFile in currentDir.GetFiles("SP_*.js"))
            {
                string storedProecureId = tmpFile.Name.Substring(3, (tmpFile.Name.Length - 6));
                Console.WriteLine("Found File: " + storedProecureId);

                var storedProc = new StoredProcedure()
                {
                    Id = storedProecureId,
                    Body = File.ReadAllText(tmpFile.FullName)
                };

                TryDeleteStoredProcedure(collection.SelfLink, storedProc.Id); 

                client.CreateStoredProcedureAsync(collection.SelfLink, storedProc, null).Wait();
            }
        }

        static void TryDeleteStoredProcedure(string collectionLink, string storedProc)
        {
            StoredProcedure sproc = client.CreateStoredProcedureQuery(collectionLink).Where(s => s.Id == storedProc).AsEnumerable().FirstOrDefault();
            if (sproc != null)
            {
                client.DeleteStoredProcedureAsync(sproc.SelfLink).Wait();
            }
        }

    }
}
