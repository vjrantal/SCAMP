var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Common;
(function (Common) {
    (function (ResourceType) {
        ResourceType[ResourceType["None"] = 0] = "None";
        ResourceType[ResourceType["VirtualMachine"] = 1] = "VirtualMachine";
        ResourceType[ResourceType["WebApp"] = 2] = "WebApp"; // Azure Web App
    })(Common.ResourceType || (Common.ResourceType = {}));
    var ResourceType = Common.ResourceType;
    (function (WebAppState) {
        WebAppState[WebAppState["None"] = 0] = "None";
        WebAppState[WebAppState["Allocated"] = 1] = "Allocated";
        WebAppState[WebAppState["Starting"] = 2] = "Starting";
        WebAppState[WebAppState["Running"] = 3] = "Running";
        WebAppState[WebAppState["Stopping"] = 4] = "Stopping";
        WebAppState[WebAppState["Stopped"] = 5] = "Stopped";
        WebAppState[WebAppState["Suspended"] = 6] = "Suspended";
        WebAppState[WebAppState["Deleting"] = 7] = "Deleting"; // the vm is being deleted, when complete it will be in an allocated state
    })(Common.WebAppState || (Common.WebAppState = {}));
    var WebAppState = Common.WebAppState;
    (function (VirtualMachineState) {
        VirtualMachineState[VirtualMachineState["None"] = 0] = "None";
        VirtualMachineState[VirtualMachineState["Allocated"] = 1] = "Allocated";
        VirtualMachineState[VirtualMachineState["Starting"] = 2] = "Starting";
        VirtualMachineState[VirtualMachineState["Running"] = 3] = "Running";
        VirtualMachineState[VirtualMachineState["Stopping"] = 4] = "Stopping";
        VirtualMachineState[VirtualMachineState["Stopped"] = 5] = "Stopped";
        VirtualMachineState[VirtualMachineState["Suspended"] = 6] = "Suspended";
        VirtualMachineState[VirtualMachineState["Deleting"] = 7] = "Deleting"; // the vm is being deleted, when complete it will be in an allocated state
    })(Common.VirtualMachineState || (Common.VirtualMachineState = {}));
    var VirtualMachineState = Common.VirtualMachineState;
    (function (RoleType) {
        RoleType[RoleType["None"] = 0] = "None";
        RoleType[RoleType["User"] = 1] = "User";
        RoleType[RoleType["GroupManager"] = 2] = "GroupManager";
        RoleType[RoleType["GroupAdmin"] = 3] = "GroupAdmin";
        RoleType[RoleType["SystemAdmin"] = 4] = "SystemAdmin";
    })(Common.RoleType || (Common.RoleType = {}));
    var RoleType = Common.RoleType;
    (function (VirtualMachineAction) {
        VirtualMachineAction[VirtualMachineAction["None"] = 0] = "None";
        VirtualMachineAction[VirtualMachineAction["Start"] = 1] = "Start";
        VirtualMachineAction[VirtualMachineAction["Stop"] = 2] = "Stop";
        VirtualMachineAction[VirtualMachineAction["Create"] = 3] = "Create";
        VirtualMachineAction[VirtualMachineAction["Delete"] = 4] = "Delete";
    })(Common.VirtualMachineAction || (Common.VirtualMachineAction = {}));
    var VirtualMachineAction = Common.VirtualMachineAction;
    var Identity = (function () {
        function Identity() {
        }
        return Identity;
    })();
    Common.Identity = Identity;
    var GroupId = (function (_super) {
        __extends(GroupId, _super);
        function GroupId() {
            _super.apply(this, arguments);
        }
        return GroupId;
    })(Identity);
    Common.GroupId = GroupId;
    var ResourceId = (function (_super) {
        __extends(ResourceId, _super);
        function ResourceId() {
            _super.apply(this, arguments);
        }
        return ResourceId;
    })(Identity);
    Common.ResourceId = ResourceId;
    var UserId = (function (_super) {
        __extends(UserId, _super);
        function UserId() {
            _super.apply(this, arguments);
        }
        return UserId;
    })(Identity);
    Common.UserId = UserId;
    var TemplateId = (function (_super) {
        __extends(TemplateId, _super);
        function TemplateId() {
            _super.apply(this, arguments);
        }
        return TemplateId;
    })(Identity);
    Common.TemplateId = TemplateId;
    var AzureSettingsId = (function (_super) {
        __extends(AzureSettingsId, _super);
        function AzureSettingsId() {
            _super.apply(this, arguments);
        }
        return AzureSettingsId;
    })(Identity);
    Common.AzureSettingsId = AzureSettingsId;
    var ScampSettingsId = (function (_super) {
        __extends(ScampSettingsId, _super);
        function ScampSettingsId() {
            _super.apply(this, arguments);
        }
        return ScampSettingsId;
    })(Identity);
    Common.ScampSettingsId = ScampSettingsId;
    var ResourceDataId = (function (_super) {
        __extends(ResourceDataId, _super);
        function ResourceDataId() {
            _super.apply(this, arguments);
        }
        return ResourceDataId;
    })(Identity);
    Common.ResourceDataId = ResourceDataId;
    var User = (function (_super) {
        __extends(User, _super);
        function User() {
            _super.apply(this, arguments);
        }
        return User;
    })(UserId);
    Common.User = User;
    var Resource = (function (_super) {
        __extends(Resource, _super);
        function Resource() {
            _super.apply(this, arguments);
        }
        return Resource;
    })(ResourceId);
    Common.Resource = Resource;
    var Template = (function (_super) {
        __extends(Template, _super);
        function Template() {
            _super.apply(this, arguments);
        }
        return Template;
    })(TemplateId);
    Common.Template = Template;
    var UserAllocation = (function () {
        function UserAllocation() {
        }
        return UserAllocation;
    })();
    Common.UserAllocation = UserAllocation;
    var Group = (function (_super) {
        __extends(Group, _super);
        function Group() {
            _super.apply(this, arguments);
        }
        return Group;
    })(GroupId);
    Common.Group = Group;
    var AzureSettings = (function (_super) {
        __extends(AzureSettings, _super);
        function AzureSettings() {
            _super.apply(this, arguments);
        }
        return AzureSettings;
    })(AzureSettingsId);
    Common.AzureSettings = AzureSettings;
    var ScampSettings = (function (_super) {
        __extends(ScampSettings, _super);
        function ScampSettings() {
            _super.apply(this, arguments);
        }
        return ScampSettings;
    })(ScampSettingsId);
    Common.ScampSettings = ScampSettings;
    var ResourceData = (function (_super) {
        __extends(ResourceData, _super);
        function ResourceData() {
            _super.apply(this, arguments);
        }
        return ResourceData;
    })(ResourceDataId);
    Common.ResourceData = ResourceData;
    var ResourceView = (function () {
        function ResourceView() {
        }
        return ResourceView;
    })();
    Common.ResourceView = ResourceView;
    var UserSummaryView = (function () {
        function UserSummaryView() {
        }
        return UserSummaryView;
    })();
    Common.UserSummaryView = UserSummaryView;
    var GroupSummaryView = (function () {
        function GroupSummaryView() {
        }
        return GroupSummaryView;
    })();
    Common.GroupSummaryView = GroupSummaryView;
    var GroupAdminSummaryView = (function () {
        function GroupAdminSummaryView() {
        }
        return GroupAdminSummaryView;
    })();
    Common.GroupAdminSummaryView = GroupAdminSummaryView;
})(Common || (Common = {}));
var Messages;
(function (Messages) {
    (function (MessageType) {
        MessageType[MessageType["Empty"] = 0] = "Empty";
        MessageType[MessageType["Error"] = 1] = "Error";
        MessageType[MessageType["Subscribe"] = 2] = "Subscribe";
        MessageType[MessageType["Update"] = 3] = "Update";
        MessageType[MessageType["GetSubscriptions"] = 4] = "GetSubscriptions";
        MessageType[MessageType["Subscriptions"] = 5] = "Subscriptions";
    })(Messages.MessageType || (Messages.MessageType = {}));
    var MessageType = Messages.MessageType;
    var Message = (function () {
        function Message(type) {
            this.Type = type;
        }
        return Message;
    })();
    Messages.Message = Message;
    var Subscribe = (function (_super) {
        __extends(Subscribe, _super);
        function Subscribe(user) {
            _super.call(this, 2 /* Subscribe */);
            this.User = user;
        }
        return Subscribe;
    })(Message);
    Messages.Subscribe = Subscribe;
    (function (UpdateActions) {
        UpdateActions[UpdateActions["None"] = 0] = "None";
        UpdateActions[UpdateActions["StateChange"] = 1] = "StateChange";
    })(Messages.UpdateActions || (Messages.UpdateActions = {}));
    var UpdateActions = Messages.UpdateActions;
    var Update = (function (_super) {
        __extends(Update, _super);
        function Update(action, user, resource, state, date) {
            if (action === void 0) { action = 0 /* None */; }
            _super.call(this, 3 /* Update */);
            this.Action = action;
            this.User = user;
            this.Resource = resource;
            this.State = state;
            this.Date = date;
        }
        return Update;
    })(Message);
    Messages.Update = Update;
    var GetSubscriptions = (function (_super) {
        __extends(GetSubscriptions, _super);
        function GetSubscriptions() {
            _super.call(this, 4 /* GetSubscriptions */);
        }
        return GetSubscriptions;
    })(Message);
    Messages.GetSubscriptions = GetSubscriptions;
    var Subscriptions = (function (_super) {
        __extends(Subscriptions, _super);
        function Subscriptions() {
            _super.call(this, 5 /* Subscriptions */);
        }
        return Subscriptions;
    })(Message);
    Messages.Subscriptions = Subscriptions;
    var Error = (function (_super) {
        __extends(Error, _super);
        function Error(e) {
            _super.call(this, 1 /* Error */);
            this.Text = e;
        }
        return Error;
    })(Message);
    Messages.Error = Error;
})(Messages || (Messages = {}));
var Search;
(function (Search) {
    var User = (function () {
        function User() {
        }
        return User;
    })();
    Search.User = User;
})(Search || (Search = {}));
var Views;
(function (Views) {
    var ResourceView = (function () {
        function ResourceView() {
        }
        return ResourceView;
    })();
    Views.ResourceView = ResourceView;
    var UserSummaryView = (function () {
        function UserSummaryView() {
        }
        return UserSummaryView;
    })();
    Views.UserSummaryView = UserSummaryView;
    var GroupSummaryView = (function () {
        function GroupSummaryView() {
        }
        return GroupSummaryView;
    })();
    Views.GroupSummaryView = GroupSummaryView;
    var GroupAdminSummaryView = (function () {
        function GroupAdminSummaryView() {
        }
        return GroupAdminSummaryView;
    })();
    Views.GroupAdminSummaryView = GroupAdminSummaryView;
})(Views || (Views = {}));
