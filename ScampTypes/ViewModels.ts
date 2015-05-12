module ViewModels {

    export enum ResourceType {
        None,
        VirtualMachine,  // Azure Virtual Machine
        WebApp           // Azure Web App
    }

    export enum ResourceState {
        None,       // default no known state
        Allocated,  // the resource can be created, but doesn't yet exist
        Starting,   // the resource is being started, if its the first time, this also means provisioning
        Running,    // the resource is running/active
        Stopping,   // the resource is being stopped
        Stopped,    // the resource is stopped, but may still be incuring charges
        Suspended,  // the resource has exceeded its usage quota
        Deleting    // the resource is being deleted, when complete it will be in an allocated state
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
        Stop,
        Start
    }

    export enum ResourceAction {
        Undefined,
        Stop,
        Start,
        Create,
        Delete
    }

    // unique id and descriptive name
    export class RefId {
        Id: string
    }

    export class Identity extends RefId {
        Name: string;
    }

    export class GroupId extends RefId { }

    export class ResourceId extends RefId { }

    export class UserId extends RefId { }

    export class TemplateId extends RefId { }

    export class AzureSettingsId extends RefId { }

    export class User extends Identity {
        Email: string;
        DisplayName: string;
    }

    export class ResourceUnits {
        Group: GroupId;
        User: UserId;
        Used: number;
    }

    export class Resource extends Identity {
        Type: ResourceType;
        Owner: UserId;
        Group: GroupId;
        Template: TemplateId;
    }

    export class Template extends Identity {
        Content: string;
        Settings: AzureSettingsId;
    }

    export class UnitsData {
        Allocated: number;
        Used: number;
    }

    export class GroupUser extends UnitsData {
        User: UserId;
        Role: RoleType;
    }

    export class GroupAdmin extends UnitsData {
        User: UserId;
    }

    export class Group extends Identity {
        Templates: TemplateId[];
        Owner: UserId;
        Users: GroupUser[];
        Units: UnitsData;
    }

    export class AzureSettings extends Identity {
        SubscriptionId: string;
        TenantId: string;
        ClientId: string;
        ExtraQueryParameter: string;
        CacheLocation: string;
        RedirectUri: string;
        ManagementThumbnail: string;
        AdminUser: string; //TODO: move to keyvault
        AdminPassword: string; //TODO: move to keyvault
    }

    export class UserRole {
        User: User;
        Role: RoleType;
    }

    export class ScampSettings extends Identity {
        GroupAdmins: GroupAdmin[];
        Users: UserRole[];
    }
}