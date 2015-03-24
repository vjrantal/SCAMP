using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Framework.ConfigurationModel;

namespace ProvisioningJobConsole
{
    public class Program
    {
        public static  IConfiguration Configuration { get; set; }
        static void Main()
        {
            // Setup configuration sources.
            Configuration = new Configuration()
                .AddEnvironmentVariables("APPSETTING_");

            var storageCstr = GetConnectionString();

            var host = new JobHost(new JobHostConfiguration(storageCstr));

            //AzureProvisioningLibrary.WebJobController w = new WebJobController();
            //w.SubmitActionInQueue(1, ResourceAction.Start);


            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        private static string GetConnectionString()
        {

            var  rv = Configuration["Provisioning:StorageConnectionString"];


            if (string.IsNullOrEmpty(rv))
            {
                throw new ArgumentNullException(@"you're missing StorageConnectionString in either ENV or Config");

            }


            return rv;
        }
    }
}
