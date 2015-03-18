using System;
using System.Collections.Generic;

namespace ScampApi.ViewModels
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // TODO - add hypermedia link
        public List<string> UserGroupDetail { get;set }
        public List<string> UserResourceDetail { get; set }
    }
}