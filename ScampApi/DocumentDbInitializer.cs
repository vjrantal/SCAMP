using System.Threading.Tasks;
using DocumentDbRepositories.Implementation;

namespace ScampApi
{
    public class DocumentDbInitializer
    {
        private RepositoryFactory _repoFactory;

        public DocumentDbInitializer(RepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;
        }

        public async Task Initialize()
        {
            await _repoFactory.GetGroupRepositoryAsync(); // Factory calls initialize :-)
        }
    }
}