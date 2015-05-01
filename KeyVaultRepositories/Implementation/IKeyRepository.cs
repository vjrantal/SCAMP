using System.Threading.Tasks;

namespace KeyVaultRepositories.Implementation
{
    public interface IKeyRepository
    {
        Task<string>  GetSecret(string resourceId, string key);
        Task UpsertSecret(string resourceId, string key, string value);
        Task DeleteSecret(string resourceId, string key);
    }
}