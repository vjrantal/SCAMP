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
            return _urlHelper.Action("Get", "Groups", new { id = groupId });
        }
    }
}