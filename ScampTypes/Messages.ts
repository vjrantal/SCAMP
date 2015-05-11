module Messages {
    export enum MessageType {
        Empty = 0,
        Error,
        Subscribe,
        Update,
        GetSubscriptions,
        Subscriptions
    }

    export class Message {
        Type: MessageType;

        constructor(type: MessageType) {
            this.Type = type;
        }
    }

    export class Subscribe extends Message {
        User: string;

        constructor(user?: string) {
            super(MessageType.Subscribe);
            this.User = user;
        }
    }

    export enum UpdateActions {
        None,
        StateChange
    }

    export class Update extends Message {
        Action: UpdateActions;
        User: string;
        Resource: string;
        State: string;
        Date: Date;

        constructor(action = UpdateActions.None, user?:string, resource?:string, state?:string, date?:Date) {
            super(MessageType.Update);

            this.Action = action;
            this.User = user;
            this.Resource = resource;
            this.State = state;
            this.Date = date;
        }
    }

    export class GetSubscriptions extends Message {
        constructor() {
            super(MessageType.GetSubscriptions);
        }
    }

    export class Subscriptions extends Message {
        List: string[];

        constructor() {
            super(MessageType.Subscriptions);
        }
    }

    export class Error extends Message {
        Text: string;

        constructor(e?: string) {
            super(MessageType.Error);
            this.Text = e;
        }
    }
}