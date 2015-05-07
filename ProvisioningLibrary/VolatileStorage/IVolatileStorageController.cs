using System;
using ProvisioningLibrary;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProvisioningLibrary
{
    public interface IVolatileStorageController 
    {
        Task<List<ActivityLog>> GetRecentActivityLogForRequest(string requestId);
        Task<List<ActivityLog>> GetRecentActivityLogForResource(string resouceId); 
        Task CreateActivityLog(ActivityLog activityLog);
        Task<int> GetResourceState(string resourceId);
        Task UpdateResourceState(CurrentResourceState newstate);

        Task CreateActivityLog(List<ActivityLog> activityLogs);
    }
}
