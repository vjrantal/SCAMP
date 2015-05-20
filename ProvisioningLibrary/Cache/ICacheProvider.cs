using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Collections.Generic;
using DocumentDbRepositories;

namespace ProvisioningLibrary
{
    public interface ICacheProvider 
    {
        Task<ScampUser> GetUser(string userId);

        Task SetUser(ScampUser userDoc);
    }
}
