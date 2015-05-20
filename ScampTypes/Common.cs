using System;
using System.Collections.Generic;

namespace ScampTypes.Common
{
    public enum ResourceType
    {
        None,
        VirtualMachine,  // Azure Virtual Machine
        WebApp           // Azure Web App
    }

    public enum WebAppState
    {
        None,       // default no known state
        Allocated,  // the vm can be created, but doesn't yet exist
        Starting,   // the vm is being started, if its the first time, this also means provisioning
        Running,    // the vm is running/active
        Stopping,   // the vm is being stopped
        Stopped,    // the vm is stopped, but may still be incuring charges
        Suspended,  // the vm has exceeded its usage quota
        Deleting    // the vm is being deleted, when complete it will be in an allocated state
    }

    public enum VirtualMachineState
    {
        None,       // default no known state
        Allocated,  // the vm can be created, but doesn't yet exist
        Starting,   // the vm is being started, if its the first time, this also means provisioning
        Running,    // the vm is running/active
        Stopping,   // the vm is being stopped
        Stopped,    // the vm is stopped, but may still be incuring charges
        Suspended,  // the vm has exceeded its usage quota
        Deleting    // the vm is being deleted, when complete it will be in an allocated state
    }

    public enum RoleType
    {
        None,
        User,
        GroupManager,
        GroupAdmin,
        SystemAdmin
    }

    public enum VirtualMachineAction
    {
        None,
        Start,
        Stop,
        Create,
        Delete
    }

    public interface IIdentity<T>
    {
        string Id { get; }
        string Name { get; }
    }

    public abstract class Identity<T> : IIdentity<T>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get { return this.GetType().Name; } }
    }

    public class GroupId : Identity<Group> { }
    public class ResourceId : Identity<Resource> { }

    public class UserId : Identity<User> { }

    public class TemplateId : Identity<Template> { }

    public class AzureSettingsId : Identity<AzureSettings> { }

    public class ScampSettingsId : Identity<ScampSettings> { }

    public class ResourceDataId : Identity<ResourceData> { }

    public sealed class User : UserId
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsAdmin { get; set; }
        public List<GroupId> Groups { get; set; }
        public List<ResourceId> Resources { get; set; }
    }

    public sealed class Resource : ResourceId
    {
        public ResourceType ResourceType { get; set; }
        public UserId Owner { get; set; }
        public GroupId Group { get; set; }
        public TemplateId Template { get; set; }
        public ResourceDataId Data { get; set; }
    }

    public sealed class Template : TemplateId
    {
        public string Content { get; set; }
        public AzureSettingsId Settings { get; set; }
    }

    public sealed class UserAllocation
    {
        public UserId User { get; set; }
        public double Budget { get; set; }
    }

    public interface IBudget
    {
        double Budget { get; set; }
        double DefaultBudget { get; set; }
        List<UserAllocation> Allocations { get; set; }
    }

    public sealed class Group : GroupId
    {
        public List<TemplateId> Templates { get; set; }
        public UserId Owner { get; set; }
        public double Budget { get; set; }
        public double DefaultBudget { get; set; }
        public List<UserAllocation> Allocations { get; set; }
    }

    public sealed class AzureSettings : AzureSettingsId
    {
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ExtraQueryParameter { get; set; }
    }

    public sealed class ScampSettings : ScampSettingsId
    {
        public UserId Owner { get; set; }
        public double Budget { get; set; }
        public double DefaultBudget { get; set; }
        public List<UserAllocation> Allocations { get; set; }
    }

    public interface ISummaryView
    {
        double Budget { get; set; }
        double Usage { get; set; }
        double Remaining { get; set; }
    }

    public sealed class ResourceData : ResourceDataId
    {
        public GroupId Group { get; set; }
        public ResourceId Resource { get; set; }
        public double Usage { get; set; }
        public string State { get; set; }
        public string ServiceName { get; set; }
        public string MachineName { get; set; }
        public string DeploymentName { get; set; }
    }
}