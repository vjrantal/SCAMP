using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;


namespace ProvisioningLibrary
{
    public static class VolatileStorageExtentions
    {
        public static void UseVolatileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IVolatileStorageController, VolatileStorageController>();
        }

    }
}
