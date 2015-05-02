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
//# sourceMappingURL=messages.js.map