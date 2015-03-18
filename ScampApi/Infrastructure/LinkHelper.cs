using System;
using Microsoft.AspNet.Mvc;

namespace ScampApi.Infrastructure
{
    public class LinkHelper : ILinkHelper
    {
        private IUrlHelper _urlHelper;

        public LinkHelper(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string Group(int groupId)
        {
            return FullyQualify(_urlHelper.RouteUrl("Groups.GetSingle", new { groupId }));
        }

        public string GroupResource(int groupId, int resourceId)
        {
            return FullyQualify(_urlHelper.RouteUrl("GroupResources.GetSingle", new { groupId, resourceId }));
        }
        public string GroupUser(int groupId, int userId)
        {
            return FullyQualify(_urlHelper.RouteUrl("GroupUsers.GetSingle", new { groupId , userId }));
        }

        private string FullyQualify(string url)
        {
            // TODo - put proper implementation here ;-)
            return "http://localhost:10838" + url;
        }
    }
}