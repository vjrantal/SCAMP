
using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Framework.ConfigurationModel;
using ProvisioningLibrary;
using ProvisioningJobConsole;
using System.Threading.Tasks;

namespace ProvisioningJobConsole
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void handleUnObservedExceptions(object sender, UnobservedTaskExceptionEventArgs eventArgs)
        {
            eventArgs.SetObserved();
            ((AggregateException)eventArgs.Exception).Handle(ex =>
            {
                Console.WriteLine("Unobserved exception type: [{0}] Message[{1}] ", ex.GetType(), ex.Message);
                return true;
            });
        }

    static void Main()
        {
            // setup unobserved exceptions
            TaskScheduler.UnobservedTaskException += Program.handleUnObservedExceptions;



         // Setup configuration sources.
        Configuration = new Configuration()
                .AddEnvironmentVariables("APPSETTING_");

            var storageCstr = GetConnectionString();

            JobHostConfiguration config = new JobHostConfiguration(storageCstr);
            config.Queues.BatchSize = 1; //Number of messages parallel processed in parallel. Will need some concurrency check before increasing.
            config.Queues.MaxDequeueCount = 4;
            config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(15);
            JobHost host = new JobHost(config);
            Console.WriteLine("Web Job starting..");

            // TEST Lines
            //ProvisioningLibrary.WebJobController w = new WebJobController(Configuration);
            //w.SubmitActionInQueue("8fd8fbfc-8fb6-4e95-ae17-aa4779423cb8", ResourceAction.Start );

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        private static string GetConnectionString()
        {

            var rv = Configuration["Provisioning:StorageConnectionString"];


            if (string.IsNullOrEmpty(rv))
            {
                throw new ArgumentNullException(@"you're missing StorageConnectionString in either ENV or Config");

            }


            return rv;
        }
    }
}
