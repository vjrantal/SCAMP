
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
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace ProvisioningLibrary
{
    /// <summary>
    /// AD Token serialization format
    /// </summary>
    [DataContract]
    internal class AADTokenFormat
    {
        [DataMember]
        internal string token_type { get; set; }
        [DataMember]
        internal string access_token { get; set; }
    }

    //[Serializable]
    //public class SearchResults 
    //{
    //    public SearchResults()
    //    {
    //        values = new List<string>();
    //    }

    //    [JsonProperty(PropertyName = "odata.metadata")]
    //    public string metadata { get; set; }

    //    [JsonProperty(PropertyName = "value")]
    //    public List<string> values { get; set; }
    //}

    [DataContract]
    internal class SearchResults
    {
        [DataMember]
        [JsonProperty(PropertyName = "odata.metadata")]
        internal string metadata { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = "value")]
        internal List<AADUser> value { get; set; }
    }

    [DataContract]
    internal class AADUser
    {
        [DataMember]
        internal string userPrincipalName { get; set; }
        [DataMember]
        internal string displayName { get; set; }
        [DataMember]
        internal string objectId { get; set; }
    }

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
                if (string.IsNullOrEmpty(GraphAPIProvider.Configuration[_cfgPropertyClientId]))
                    throw new InvalidOperationException(@"GraphAPIProvider couldn't load config setting " + _cfgPropertyClientId + " from either ENV or Config");
                if (string.IsNullOrEmpty(GraphAPIProvider.Configuration[_cfgPropertyClientSecret]))
                    throw new InvalidOperationException(@"GraphAPIProvider couldn't load config setting " + _cfgPropertyClientSecret + " from either ENV or Config");
            }

            Trace.WriteLine("a new instance of GraphAPIProvider has been created.");
        }

        /// <summary>
        /// retrieves an AAD tenant security token
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAADToken()
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(AADTokenFormat));
            string rtnValue = string.Empty;

            WebRequest request = WebRequest.Create(String.Format("https://login.windows.net/{0}/oauth2/token?api-version=1.0", 
                GraphAPIProvider.Configuration[_cfgPropertyTenantId]));
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            string postData = string.Format("grant_type=client_credentials&resource={0}&client_id={1}&client_secret={2}", 
                HttpUtility.UrlEncode("https://graph.windows.net"),
                HttpUtility.UrlEncode(GraphAPIProvider.Configuration[_cfgPropertyClientId]),
                HttpUtility.UrlEncode(GraphAPIProvider.Configuration[_cfgPropertyClientSecret]));

            byte[] data = encoding.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                stream.Write(data, 0, data.Length);
            }

            // get token and return it as a string
            using (var response = await request.GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);

                    try
                    {
                        AADTokenFormat token = (AADTokenFormat)(ser.ReadObject(stream));

                        rtnValue = string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                                            token.token_type, token.access_token);
                    }
                    catch(Exception ex)
                    {
                        string tmp = ex.Message;
                    }

                }
            }

            return rtnValue;
        }

        /// <summary>
        /// Tries to find a user based on a search string
        /// </summary>
        /// <param name="search">search value to use</param>
        /// <returns>returns a match if one if found, otherwise returns null</returns>
        public async Task<UserSummary> FindUser(string search)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SearchResults));

            // create search URI
            string searchURI = string.Format("https://graph.windows.net/{0}/users?api-version=2013-04-05&$top=1&$filter=startswith(userPrincipalName,'{1}')",
                GraphAPIProvider.Configuration[_cfgPropertyTenantId], 
                search);
            // create event handler for response
            WebClient client = new WebClient();
            
            //TODO: need to cache token until it expires and simply reuse
            client.Headers.Add("Authorization", await GetAADToken());

            string searchResults = await client.DownloadStringTaskAsync(new Uri(searchURI));
            //SearchResults tmpResults = JsonConvert.DeserializeObject<SearchResults>(searchResults);

            byte[] byteArray = Encoding.ASCII.GetBytes(searchResults);
            MemoryStream stream = new MemoryStream(byteArray);

            SearchResults srcResults = (SearchResults)ser.ReadObject(new MemoryStream(byteArray));

            if (srcResults.value.Count > 0)
                return new UserSummary()
                {
                    Id = srcResults.value[0].objectId,
                    Name = srcResults.value[0].displayName
                };
            else
                return null;
        }
    }
}
