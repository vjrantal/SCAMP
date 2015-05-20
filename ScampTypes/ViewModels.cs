using System;
using System.Collections.Generic;

namespace ScampTypes.ViewModels
{
    public sealed class Link
    {
    public string Rel { get; set; }
    public string Href { get; set; }
    }

    public sealed class UserGroupSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double totUnitsUsed { get; set; }
        public double totUnitsRemaining { get; set; }
    }

    public sealed class UserSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public sealed class UserUsageSummary
    {
        public double totUnitsAllocated { get; set; }
        public double unitsBudgeted { get; set; }
        public double totUnitsUsed { get; set; }
        public int totGroupMemberships { get; set; }
    }

    public sealed class GroupResource
    {
    public string Id { get; set; }
    public string Name { get; set; }
    public string ResourceId { get; set; }
    public List<UserSummary> Users { get; set; }
    }

    public enum ResourceType
    {
        None,
        VirtualMachine,  // Azure Virtual Machine
        WebApp           // Azure Web App
    }

    public enum ResourceState
    {
        Unknown,    // default no known state
        Allocated,  // the resource can be created, but doesn't yet exist
        Starting,   // the resource is being started, if its the first time, this also means provisioning
        Running,    // the resource is running/active
        Stopping,   // the resource is being stopped
        Stopped,    // the resource is stopped, but may still be incuring charges
        Suspended,  // the resource has exceeded its usage quota
        Deleting    // the resource is being deleted, when complete it will be in an allocated state
    }

    public class ScampResourceGroupReference
    {
    public string Id { get; set; }
    public string Name { get; set; }
    }

    public class ScampAdminGroupReference : ScampResourceGroupReference
    {
        public double totUnitsUsed { get; set; }
        public double totUnitsAllocated { get; set; }
        public double totUnitsBudgeted { get; set; }
    }

    public class ScampUserGroupReference : ScampResourceGroupReference
    {
        public double totUnitsUsedByUser { get; set; }
        public double totUnitsRemainingForUser { get; set; }
    }

    public sealed class ScampResourceSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ResourceType Type { get; set; }
        public ResourceState State { get; set; }
        public double totUnitsUsed { get; set; }
    }

    [Serializable]
    public sealed class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Group> Groups { get; set; }
        public ScampResourceSummary Resources { get; set; }
    }

    public sealed class GroupTemplate
    {
    public string Id { get; set; }
    public string Name { get; set; }
    public string GroupId { get; set; }
    }

    public sealed class GroupTemplateSummary
    {
    public string Id { get; set; }
    public string Name { get; set; }
    public string TemplateId { get; set; }
    public List<Link> Links { get; set; }
    }

    public sealed class Group
    {
    public string Id { get; set; }
    public string Name { get; set; }
    public List<ScampResourceSummary> Resources{ get; set; }
    public List<GroupTemplateSummary> Templates { get; set; }
    public List<UserSummary> Admins { get; set; }
    public List<UserSummary> Members { get; set; }
    }

    public sealed class GroupSummary
    {
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Link> Links { get; set; }
    }

    public sealed class ScampSettings
    {
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string ExtraQueryParameter { get; set; }
    public string CacheLocation { get; set; }
    public string RedirectUri { get; set; }
    }
}
