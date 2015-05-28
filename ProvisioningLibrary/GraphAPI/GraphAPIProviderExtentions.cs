using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;


namespace ProvisioningLibrary
{
    public static class GraphAPIProviderExtentions
    {
        public static void AddGraphAPIProvider(this IServiceCollection services, IConfiguration configuration)
        {
            GraphAPIProvider graphPrvdr = new GraphAPIProvider(configuration);
            services.AddInstance<GraphAPIProvider>(graphPrvdr);
            services.AddTransient<IGraphAPIProvider, GraphAPIProvider> ();
        }

    }
}
