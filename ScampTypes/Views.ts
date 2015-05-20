module Views {

    export interface ISummaryView {
        Budget: number;
        Usage: number;
        Remaining: number;
    }

    export class ResourceView {
        Resource: Common.Resource;
        Data: Common.ResourceData;
    }

    export class UserSummaryView implements ISummaryView {
        User: Common.UserId;
        Group: Common.Group;
        Resources: ResourceView[];
        Budget: number;
        Usage: number;
        Remaining: number;
    }

    export class GroupSummaryView implements ISummaryView {
        Group: Common.Group;
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