using System.Threading.Tasks;
using DocumentDbRepositories;

namespace ScampApi.Infrastructure
{
    public interface ISecurityHelper
    {
        //string GetIPIDByContext();
        Task<ScampUser> GetCurrentUser();
        Task<ScampUser> GetUserById(string IPID);
        Task<ScampUserReference> GetUserReference();
        Task<bool> IsGroupAdmin(string groupId);
        Task<bool> IsSysAdmin();
    }
}