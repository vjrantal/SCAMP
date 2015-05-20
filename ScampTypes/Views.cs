using System;
using System.Collections.Generic;
using ScampTypes.Common;

namespace ScampTypes.Views
{
    public interface ISummaryView
    {
        double Budget { get; set; }
        double Usage { get; set; }
        double Remaining { get; set; }
    }

    public sealed class ResourceView
    {
        public Resource Resource { get; set; }
        public ResourceData Data { get; set; }
    }

    public sealed class UserSummaryView : ISummaryView
    {
        public UserId User { get; set; }
        public Group Group { get; set; }
        public List<ResourceView> Resources { get; set; }
        public double Budget { get; set; }
        public double Usage { get; set; }
        public double Remaining { get; set; }
    }

    public sealed class GroupSummaryView : ISummaryView
    {
        public Group Group { get; set; }
        public List<UserSummaryView> Users { get; set; }
        public double Budget { get; set; }
        public double Usage { get; set; }
        public double Remaining { get; set; }

    }

    public sealed class GroupAdminSummaryView : ISummaryView
    {
        public List<GroupSummaryView> Groups { get; set; }
        public double Budget { get; set; }
        public double Usage { get; set; }
        public double Remaining { get; set; }
    }

}