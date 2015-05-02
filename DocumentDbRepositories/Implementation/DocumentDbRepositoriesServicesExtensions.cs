using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace DocumentDbRepositories.Implementation
{
    public static class DocumentDbRepositoriesServicesExtensions
    {
        public static DocDb Current = null;
        public static void AddDocumentDbRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            if (Current == null)
            {
                Current = new DocDb(services, configuration);
            }
        }
    }
}
