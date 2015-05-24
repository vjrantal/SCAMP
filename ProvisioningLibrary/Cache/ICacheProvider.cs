using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Collections.Generic;
using ScampTypes.ViewModels;

namespace ProvisioningLibrary
{
    public interface ICacheProvider 
    {
        Task<User> GetUser(string userId);

        Task SetUser(User userDoc);
    }
}
