///
/// Code here taken from https://github.com/AzureADSamples/ConsoleApp-GraphAPI-DotNet/tree/master/GraphConsoleAppV3
/// and adapted for our use
///

using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

namespace ProvisioningLibrary
{
    internal class AuthenticationHelper
    {
        public const string ResourceUrl = "https://graph.windows.net";

        /// <summary>
        /// Async task to acquire token for Application.
        /// </summary>
        /// <returns>Async Token for application.</returns>
        public static async Task<string> AcquireTokenAsyncForApplication(string TenantName, string ClientId, string ClientSecret)
        {
            return GetTokenForApplication(TenantName, ClientId, ClientSecret);
        }

        /// <summary>
        /// Get Token for Application.
        /// </summary>
        /// <returns>Token for application.</returns>
        public static string GetTokenForApplication(string TenantName, string ClientId, string ClientSecret)
        {
            string AuthString = "https://login.microsoftonline.com/" + TenantName;

            AuthenticationContext authenticationContext = new AuthenticationContext(AuthString, false);
            // Config for OAuth client credentials 
            ClientCredential clientCred = new ClientCredential(ClientId, ClientSecret);
            AuthenticationResult authenticationResult = authenticationContext.AcquireToken(AuthenticationHelper.ResourceUrl,
                clientCred);
            string token = authenticationResult.AccessToken;
            return token;
        }

        /// <summary>
        /// Get Active Directory Client for Application.
        /// </summary>
        /// <returns>ActiveDirectoryClient for Application.</returns>
        public static ActiveDirectoryClient GetActiveDirectoryClientAsApplication(string TenantName, string TenantId, string ClientId, string ClientSecret)
        {
            ActiveDirectoryClient activeDirectoryClient = null;

            // build parameters
            Uri servicePointUri = new Uri(AuthenticationHelper.ResourceUrl);
            System.Uri serviceRoot = new System.Uri(servicePointUri, TenantId);
            Task getToken = new Task(async () => await AcquireTokenAsyncForApplication(TenantName, ClientId, ClientSecret));

            // get and return client
            //TODO: need to get this call to work... 
            //activeDirectoryClient = new ActiveDirectoryClient(new System.Uri("mystring"), getToken);
            return activeDirectoryClient;
        }

    }
}
