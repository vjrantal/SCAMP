using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;


namespace ProvisioningLibrary
{
    public static class CacheProviderExtentions
    {
        public static void AddCacheProvider(this IServiceCollection services, IConfiguration configuration)
        {
            CacheProvider cachePrvdr = new CacheProvider(configuration);
            services.AddInstance<CacheProvider>(cachePrvdr);
            services.AddTransient<ICacheProvider, CacheProvider > ();
        }

    }
}
