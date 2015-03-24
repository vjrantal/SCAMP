using System.Collections.Generic;

namespace ScampApi.ViewModels
{
    public class GroupResource
    {
        public string GroupId { get; set; }
        public string ResourceId { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserSummary> Users { get; set; }
    }
}
