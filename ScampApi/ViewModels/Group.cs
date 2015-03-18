using System.Collections.Generic;

namespace ScampApi.ViewModels
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<GroupResourceSummary> Resources { get; set; }
        public IEnumerable<GroupTemplateSummary> Templates { get; set; }
        public IEnumerable<GroupUserSummary> Users { get; set; }
    }
}
