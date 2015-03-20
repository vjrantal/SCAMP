﻿using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using ProvisioningLibrary;

namespace ProvisioningJobConsole
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("processorqueue")] QueueMessage message, TextWriter log)
        {
            //TODO Get cert from DB and SubscritpionId this is temporary
            string cert = ProvisioningLibraryConfiguration.GetStorageSubscriptionCertificate() ;
            string subscriptionId = ProvisioningLibraryConfiguration.GetStorageSubscriptionId() ;
            var resourceController  = new ResourceController(cert, subscriptionId);
            //TODO Get Connection string from DB

            if (message.Action == ResourceAction.Stop )
            {
                log.WriteLine("Stopping VM");
                var x=resourceController.StartStopVirtualMachine("VSGAB","DEVSTATION", VirtualMachineAction.Stop);
                x.Wait();
            }
            if (message.Action == ResourceAction.Start)
            {
                log.WriteLine("Starting VM");
               var x=resourceController.StartStopVirtualMachine("VSGAB", "DEVSTATION", VirtualMachineAction.Start);
                x.Wait();
            }
            if (message.Action == ResourceAction.Create )
            {
                log.WriteLine("Creating VM");
                var vmName = "gabrielc-" + (new Random()).Next(1, 1000);
                log.WriteLine("Creating Cloud Services");
                resourceController.CreateCloudService(vmName, LocationNames.NorthEurope).RunSynchronously();
                log.WriteLine("Creating Storage");
                resourceController.CreateStorageAccount(LocationNames.NorthEurope, vmName).RunSynchronously();
                log.WriteLine("Creating Virtual Machine");
                resourceController.CreateVirtualMachine(vmName, vmName, vmName, "gabrielc", "Enter.321", "Visual-Studio-2015-Ultimate", VirtualMachineRoleSize.Small).RunSynchronously();

            }
          

            log.WriteLine(message);
        }

    }

}