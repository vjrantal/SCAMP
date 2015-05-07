var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
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
    var ScampUserReference = (function () {
        function ScampUserReference() {
        }
        return ScampUserReference;
    })();
    Search.ScampUserReference = ScampUserReference;
})(Search || (Search = {}));
var ViewModels;
(function (ViewModels) {
    var Link = (function () {
        function Link() {
        }
        return Link;
    })();
    ViewModels.Link = Link;
    var UserSummary = (function () {
        function UserSummary() {
        }
        return UserSummary;
    })();
    ViewModels.UserSummary = UserSummary;
    var GroupResource = (function () {
        function GroupResource() {
        }
        return GroupResource;
    })();
    ViewModels.GroupResource = GroupResource;
    (function (ResourceType) {
        ResourceType[ResourceType["None"] = 0] = "None";
        ResourceType[ResourceType["VirtualMachine"] = 1] = "VirtualMachine";
        ResourceType[ResourceType["WebApp"] = 2] = "WebApp"; // Azure Web App
    })(ViewModels.ResourceType || (ViewModels.ResourceType = {}));
    var ResourceType = ViewModels.ResourceType;
    (function (ResourceState) {
        ResourceState[ResourceState["None"] = 0] = "None";
        ResourceState[ResourceState["Allocated"] = 1] = "Allocated";
        ResourceState[ResourceState["Starting"] = 2] = "Starting";
        ResourceState[ResourceState["Running"] = 3] = "Running";
        ResourceState[ResourceState["Stopping"] = 4] = "Stopping";
        ResourceState[ResourceState["Stopped"] = 5] = "Stopped";
        ResourceState[ResourceState["Suspended"] = 6] = "Suspended";
        ResourceState[ResourceState["Deleting"] = 7] = "Deleting"; // the resource is being deleted, when complete it will be in an allocated state
    })(ViewModels.ResourceState || (ViewModels.ResourceState = {}));
    var ResourceState = ViewModels.ResourceState;
    var ScampResourceGroupReference = (function () {
        function ScampResourceGroupReference() {
        }
        return ScampResourceGroupReference;
    })();
    ViewModels.ScampResourceGroupReference = ScampResourceGroupReference;
    var ScampResourceSummary = (function () {
        function ScampResourceSummary() {
        }
        return ScampResourceSummary;
    })();
    ViewModels.ScampResourceSummary = ScampResourceSummary;
    var User = (function () {
        function User() {
        }
        return User;
    })();
    ViewModels.User = User;
    var GroupTemplate = (function () {
        function GroupTemplate() {
        }
        return GroupTemplate;
    })();
    ViewModels.GroupTemplate = GroupTemplate;
    var GroupTemplateSummary = (function () {
        function GroupTemplateSummary() {
        }
        return GroupTemplateSummary;
    })();
    ViewModels.GroupTemplateSummary = GroupTemplateSummary;
    var Group = (function () {
        function Group() {
        }
        return Group;
    })();
    ViewModels.Group = Group;
    var GroupSummary = (function () {
        function GroupSummary() {
        }
        return GroupSummary;
    })();
    ViewModels.GroupSummary = GroupSummary;
    var ScampSettings = (function () {
        function ScampSettings() {
        }
        return ScampSettings;
    })();
    ViewModels.ScampSettings = ScampSettings;
})(ViewModels || (ViewModels = {}));
