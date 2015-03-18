using System;

namespace ScampApi.ViewModels
{
    public class UserSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserUrl { get; internal set; }
    }
}