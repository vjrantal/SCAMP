using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScampTypes.Messages
{
    public enum MessageType
    {
        Empty = 0,
        Error,
        Subscribe,
        Update,
        GetSubscriptions,
        Subscriptions
    }

    public class MessageConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Message);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject o = JObject.Load(reader);
            var type = (MessageType)o["Type"].Value<int>();

            switch (type)
            {
                case MessageType.Empty:
                    return o.ToObject<Empty>(serializer);
                case MessageType.Error:
                    return o.ToObject<Error>(serializer);
                case MessageType.Subscribe:
                    return o.ToObject<Subscribe>(serializer);
                case MessageType.Update:
                    return o.ToObject<Update>(serializer);
                case MessageType.GetSubscriptions:
                    return o.ToObject<GetSubscriptions>(serializer);
                case MessageType.Subscriptions:
                    return o.ToObject<Subscriptions>(serializer);
                default:
                    throw new ApplicationException("unhandled message type:" + type);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Message
    {
        public MessageType Type { get; set; }

        public Message(MessageType type)
        {
            this.Type = type;
        }

        public static readonly ArraySegment<byte> EmptySegment = new ArraySegment<byte>();
        public static readonly Message Empty = new Empty();

        public static readonly JsonSerializerSettings SerializerSettings =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                Converters = new[] { new MessageConverter() }
            };

        public ArraySegment<byte> Serialize()
        {
            try
            {
                string s = JsonConvert.SerializeObject(this, SerializerSettings);
                byte[] b = UTF8Encoding.UTF8.GetBytes(s);
                return new ArraySegment<byte>(b);
            }
            catch
            {
                return EmptySegment;
            }
        }

        public static Message Deserialize(ArraySegment<byte> segment)
        {
            try
            {
                string s = UTF8Encoding.UTF8.GetString(segment.Array, 0, segment.Count);
                return JsonConvert.DeserializeObject<Message>(s, Message.SerializerSettings);
            }
            catch
            {
                return Message.Empty;
            }
        }
    }

    public sealed class Subscribe : Message
    {
        public string User { get; set; }

        public Subscribe() : base(MessageType.Subscribe) { }

        public Subscribe(string user) : this()
        { 
            this.User = user;
        }
    }

    public enum UpdateActions
    {
        None,
        StateChange
    }

    public class Update : Message
    {
        public UpdateActions Action { get; set; }
        public string User { get; set; }
        public string Resource { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }

        public Update() : base(MessageType.Update) { this.Action = UpdateActions.None; }

        public Update(UpdateActions action, string user, string resource, string state, DateTime date) : this()
        {
            this.Action = action;
            this.User = user;
            this.Resource = resource;
            this.State = state;
            this.Date = date;
        }
    }

    public class Error :  Message
    {
        public string Text { get; set; }

        public Error() : base(MessageType.Error) { }

        public Error(string e) : this()
        {
            this.Text = e;
        }
    }

    public class GetSubscriptions : Message
    {
        public GetSubscriptions() : base(MessageType.GetSubscriptions) {  }
    }

    public class Subscriptions : Message
    {
        public List<string> List { get; set; }
        public Subscriptions() : base(MessageType.Subscriptions) { }
    }

    public class Empty : Message
    {
        public Empty() : base(MessageType.Empty) { }
    }

}