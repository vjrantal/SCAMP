using System;

namespace ProvisioningLibrary
{
    public interface IWebJobController
    {
        Guid SubmitActionInQueue(string  resourceId, ProvisioningLibrary.ResourceAction action, uint? duration = null);
        void SubmitActionInQueue(string resourceId, string actionname, uint? duration = null);
    }
}