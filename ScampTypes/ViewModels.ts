module ViewModels {

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

    export enum VirtualMachineAction {
        None,
        Start,
        Stop,
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

    export class AuthorizationGroupId extends RefId { }

    export class AllocationGroupId extends RefId { }

    export class ResourceGroupId extends RefId { }

    export class ResourceId extends RefId { }

    export class UserId extends RefId { }

    export class TemplateId extends RefId { }

    export class AzureSettingsId extends RefId { }

    export class ScampSettingsId extends RefId { }

    export class VirtualMachineAccessId extends RefId { }

    export class ResourceDataId extends RefId { }

    export interface IUnitAllocation {
        Allocation: number;
    }

    export interface IAuthorizations {
        Authorizations: AuthorizationGroupId[];
    }

    export interface IAllocationGroup {
        AllocationGroup: AllocationGroupId;
    }

    export class User extends Identity {
        Email: string;
        DisplayName: string;
    }

    export class Resource extends Identity {
        Owner: UserId;
        Type: ResourceType;
        Group: ResourceGroupId;
        Template: TemplateId;
        Data: ResourceDataId;
    }

    // TODO: move this into secret store
    export class VirtualMachineAccess extends Identity {
        Username: string;
        Password: string;
        RdpPort: number;
    }

    export class ResourceData extends Identity {
        UnitsUsed: number;
    }

    export class WebAppData extends ResourceData {
        State: WebAppState;
    }

    export class VirtualMachineData extends ResourceData {
        ServiceName: string;
        MachineName: string;
        State: VirtualMachineState;
        AccessData: VirtualMachineAccessId;
    }

    export class Template extends Identity {
        Content: string;
        Settings: AzureSettingsId;
        UnitRate: number;
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

    export class AuthorizationGroup extends Identity {
        Users: UserId[];
    }

    export class UserAllocation implements IUnitAllocation {
        User: UserId;
        Allocation: number;
    }

    export class AllocationGroup extends Identity {
        Allocations: UserAllocation[];
    }

    export class ResourceGroup extends Identity implements IUnitAllocation, IAuthorizations {
        Owner: UserId;
        Allocation: number;
        Templates: TemplateId[];
        Authorizations: AuthorizationGroupId[];
        AllocationGroup: AllocationGroupId;
    }

    export class ScampSettings extends Identity implements IUnitAllocation, IAuthorizations {
        Authorizations: AuthorizationGroupId[];
        Allocation: number;
    }
}