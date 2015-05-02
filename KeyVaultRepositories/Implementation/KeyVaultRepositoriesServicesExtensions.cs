using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace KeyVaultRepositories.Implementation
{
    public static class KeyVaultRepositoriesServicesExtensions
    {
        public static void  AddKeyVaultRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            KeyVaultScampClient kvsvc = new KeyVaultScampClient(configuration);
            services.AddInstance<KeyVaultScampClient>(kvsvc);
            services.AddTransient<IKeyRepository, KeyRepository>();
        }
    }
}