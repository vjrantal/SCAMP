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
            if (args.Count() < 4)
            {
                Console.WriteLine("You must specify the following parameters: Document DB endpoint URL, secret key, database name, and collection name.");
                Console.WriteLine("Program terminated. Press any key to exit");
                Console.ReadKey();
                return; // exit program

            }

            // setup DocumentDB client using passed parameters
            Uri endpoint = new Uri(args[0]);
            string authKey = args[1];
            string databaseId = args[2];
            string collectionId = args[3];

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

            Console.WriteLine("Stored Procedure Setup completed. Press any key to continue.");
            Console.ReadKey();
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
