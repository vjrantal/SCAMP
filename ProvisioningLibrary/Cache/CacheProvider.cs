
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
using DocumentDbRepositories; 
using StackExchange.Redis;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ProvisioningLibrary
{
    /// <summary>
    /// A cache provider that implements/wraps the hosted Redis cache service provied by Azure
    ///     This implementation contains necessary methods to add/remove various data store 
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private const string _cfgPropertyConnectionString = "Cache:ConnectionString";
        private string _CacheConnectionString = null;
        private IDatabase _cache = null;

        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Creates the cache client and establishes a connection
        /// </summary>
        private void Connect()
        {
            Debug.WriteLine("Cache Provider is not connected");

            _CacheConnectionString = CacheProvider.Configuration[_cfgPropertyConnectionString];

            ConnectionMultiplexer cacheConnection = ConnectionMultiplexer.Connect(_CacheConnectionString);

            _cache = cacheConnection.GetDatabase();

            Debug.WriteLine("Cache Provider is now connected");
        }
        
        /// <summary>
        /// Constructor for the provider. Creates underlying client object and establishes connection
        /// </summary>
        /// <param name="configuration">configuration settings for application</param>
        public CacheProvider(IConfiguration configuration)
        {

            if (null == CacheProvider.Configuration)
            {
                // Setup configuration sources.
                CacheProvider.Configuration = configuration;

                // ensure necessary configuration setting(s) are present
                if (
                    string.IsNullOrEmpty(CacheProvider.Configuration[_cfgPropertyConnectionString])
                  )
                    throw new InvalidOperationException(@"couldn't load config either ENV or Config");
            }

            Connect();

            Trace.WriteLine("a new instance of CacheProvider has been created.");
        }

        #region common/shared helper methods
        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }
        #endregion  

        /// <summary>
        /// Get's a cooresponding user view object from the cache
        /// </summary>
        /// <param name="userId">Id of the user view to be retrieved</param>
        /// <returns></returns>
        public async Task<ScampUser> GetUser(string userId)
        {
            return Deserialize<ScampUser>(await _cache.StringGetAsync(userId));
        }

        /// <summary>
        /// Sets (adds) a user view object into the cache
        /// </summary>
        /// <param name="user">the user view object to be inserted</param>
        /// <returns></returns>
        public async Task SetUser(ScampUser userDoc)
        {
            try
            {
                await _cache.StringSetAsync(userDoc.Id, Serialize(userDoc));
            }
            catch (Exception ex)
            {
                //TODO: log issue
                string tmp = ex.Message; 
            }
        }

    }
}
