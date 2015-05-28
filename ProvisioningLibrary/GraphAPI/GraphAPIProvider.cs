
using System;
using ProvisioningLibrary;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Microsoft.Framework.ConfigurationModel;
using ScampTypes.ViewModels;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace ProvisioningLibrary
{
    /// <summary>
    /// A cache provider that implements/wraps the hosted Redis cache service provied by Azure
    ///     This implementation contains necessary methods to add/remove various data store 
    /// </summary>
    public class GraphAPIProvider : IGraphAPIProvider
    {
        private const string _cfgPropertyTenantName = "TenantName";
        private const string _cfgPropertyTenantId = "TenantId";
        private const string _cfgPropertyClientId = "ClientId";
        private const string _cfgPropertyClientSecret = "ClientSecret";

        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Constructor for the provider. Creates underlying client object and establishes connection
        /// </summary>
        /// <param name="configuration">configuration settings for application</param>
        public GraphAPIProvider(IConfiguration configuration)
        {
            // check configuration
            if (null == GraphAPIProvider.Configuration)
            {
                // Setup configuration sources.
                GraphAPIProvider.Configuration = configuration;

                // ensure necessary configuration setting(s) are present
                if (string.IsNullOrEmpty(GraphAPIProvider.Configuration[_cfgPropertyTenantName]))
                    throw new InvalidOperationException(@"GraphAPIProvider couldn't load config setting " + _cfgPropertyTenantName + " from either ENV or Config");
                if (string.IsNullOrEmpty(GraphAPIProvider.Configuration[_cfgPropertyTenantId]))
                    throw new InvalidOperationException(@"GraphAPIProvider couldn't load config setting " + _cfgPropertyTenantId + " from either ENV or Config");
                if (string.IsNullOrEmpty(GraphAPIProvider.Configuration[_cfgPropertyClientId]))
                    throw new InvalidOperationException(@"GraphAPIProvider couldn't load config setting " + _cfgPropertyClientId + " from either ENV or Config");
                if (string.IsNullOrEmpty(GraphAPIProvider.Configuration[_cfgPropertyClientSecret]))
                    throw new InvalidOperationException(@"GraphAPIProvider couldn't load config setting " + _cfgPropertyClientSecret + " from either ENV or Config");
            }

            // create AD client
            ActiveDirectoryClient activeDirectoryClient = AuthenticationHelper.GetActiveDirectoryClientAsApplication(
                GraphAPIProvider.Configuration[_cfgPropertyTenantName],
                GraphAPIProvider.Configuration[_cfgPropertyTenantId],
                GraphAPIProvider.Configuration[_cfgPropertyClientId],
                GraphAPIProvider.Configuration[_cfgPropertyClientSecret]
            );

                Trace.WriteLine("a new instance of GraphAPIProvider has been created.");
        }

        /// <summary>
        /// Tries to find a user based on a search string
        /// </summary>
        /// <param name="search">search value to use</param>
        /// <returns></returns>
        public async Task<UserSummary> FindUser(string search)
        {
            return new UserSummary(); 
        }
    }
}
