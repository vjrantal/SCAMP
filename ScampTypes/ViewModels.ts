module ViewModels {

    export class Link {
        Rel: string;
        Href: string;
    }

    export class UserSummary {
        UserId: string;
        Name: string;
        Links: Link[];
    }

    export class GroupResource {
        Id: string;
        Name: string;
        ResourceId: string;
        Users: UserSummary[]
    }

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

    export class ScampResourceGroupReference {
        Id: string;
        Name: string;
    }

    export class ScampResourceSummary {
        Id: string;
        Name: string;
        ResourceGroup: ScampResourceGroupReference;
        Type: ResourceType;
        State: ResourceState;
        Remaining: number;
    }

    export class User {
        Id: string;
        Name: string;
        Email: string;
        Groups: Group[];
        Resources: ScampResourceSummary[];
    }

    export class GroupTemplate {
        Id: string;
        Name: string;
        GroupId: string;
    }

    export class GroupTemplateSummary {
        Id: string;
        Name: string;
        TemplateId: string;
        Links: Link[];
    }

    export class Group {
        Id: string;
        Name: string;
        Resources: ScampResourceSummary[];
        Templates: GroupTemplateSummary[];
        Admins: UserSummary[];
        Members: UserSummary[];
    }

    export class GroupSummary {
        Id: string;
        Name: string;
        Links: Link[];
    }

    export class ScampSettings {
        TenantId: string;
        ClientId: string;
        ExtraQueryParameter: string;
        CacheLocation: string;
        RedirectUri: string;
    }
}