module Common {

    export enum ResourceType {
        None,
        VirtualMachine,  // Azure Virtual Machine
        WebApp           // Azure Web App
    }

    export enum WebAppState {
        None,       // default no known state
        Allocated,  // the vm can be created, but doesn't yet exist
        Starting,   // the vm is being started, if its the first time, this also means provisioning
        Running,    // the vm is running/active
        Stopping,   // the vm is being stopped
        Stopped,    // the vm is stopped, but may still be incuring charges
        Suspended,  // the vm has exceeded its usage quota
        Deleting    // the vm is being deleted, when complete it will be in an allocated state
    }

    export enum VirtualMachineState {
        None,       // default no known state
        Allocated,  // the vm can be created, but doesn't yet exist
        Starting,   // the vm is being started, if its the first time, this also means provisioning
        Running,    // the vm is running/active
        Stopping,   // the vm is being stopped
        Stopped,    // the vm is stopped, but may still be incuring charges
        Suspended,  // the vm has exceeded its usage quota
        Deleting    // the vm is being deleted, when complete it will be in an allocated state
    }

    export enum RoleType {
        None,
        User,
        GroupManager,
        GroupAdmin,
        SystemAdmin
    }

    export enum VirtualMachineAction {
        None,
        Start,
        Stop,
        Create,
        Delete
    }

    // unique id and descriptive name
    export interface IIdentity<T> {
        "$type": string;
        Id: string;
        Name: string;
    }

    export class Identity<T> implements IIdentity<T> {
        "$type": string;
        Id: string;
        Name: string;
    }

    export class GroupId extends Identity<Group> { }

    export class ResourceId extends Identity<Resource> { }

    export class UserId extends Identity<User> { }

    export class TemplateId extends Identity<Template> { }

    export class AzureSettingsId extends Identity<AzureSettings> { }

    export class ScampSettingsId extends Identity<ScampSettings> { }

    export class ResourceDataId extends Identity<ResourceData> { }

    export class User extends UserId {
        Email: string;
        DisplayName: string;
        IsAdmin: boolean;
        Groups: GroupId[];
        Resources: ResourceId[];
    }

    export class Resource extends ResourceId {
        ResourceType: ResourceType;
        Owner: UserId;
        Group: GroupId;
        Template: TemplateId;
        Data: ResourceDataId;
    }

    export class Template extends TemplateId {
        Content: string;
        Settings: AzureSettingsId;
    }

    export class UserAllocation {
        User: UserId;
        Budget: number;
    }

    export interface IBudget {
        Budget: number;
        DefaultBudget: number;
        Allocations: UserAllocation[];
    }

    export class Group extends GroupId implements IBudget {
        Templates: TemplateId[];
        Owner: UserId;
        Budget: number;
        DefaultBudget: number;
        Allocations: UserAllocation[];
    }

    export class AzureSettings extends AzureSettingsId {
        SubscriptionId: string;
        TenantId: string;
        ClientId: string;
        ExtraQueryParameter: string;
    }

    export class ScampSettings extends ScampSettingsId implements IBudget {
        Owner: UserId;
        Budget: number;
        DefaultBudget: number;
        Allocations: UserAllocation[];
    }

    export class ResourceData extends ResourceDataId {
        Group: GroupId;
        Resource: ResourceId;
        Usage: number;
        State: string;
        ServiceName: string;
        MachineName: string;
        DeploymentName: string;
    }

    export class ResourceView {
        Resource: Resource;
        Data: ResourceData;
    }

    export class UserSummaryView implements ISummaryView {
        User: UserId;
        Group: Group;
        Resources: ResourceView[];
        Budget: number;
        Usage: number;
        Remaining: number;
    }

    export class GroupSummaryView implements ISummaryView {
        Group: Group;
        Users: UserSummaryView[];
        Budget: number;
        Usage: number;
        Remaining: number;
    }

    export class GroupAdminSummaryView implements ISummaryView {
        Groups: GroupSummaryView[];
        Budget: number;
        Usage: number;
        Remaining: number;
    }
}