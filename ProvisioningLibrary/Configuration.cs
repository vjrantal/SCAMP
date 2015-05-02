using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace ProvisioningLibrary
{
    public static class ProvisioningServicesExtensions
    {
        public static void AddProvisioning(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IResourceController, ResourceController>();
        }
    }
}