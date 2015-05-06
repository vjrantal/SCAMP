using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.WebSockets.Server;
using Microsoft.AspNet.WebSockets.Protocol;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ScampTypes.Messages;
using System.Collections.Generic;
using System.Net.WebSockets;


namespace Monitor
{
    public interface ISessionHandler
    {
        Task MessageLoopAsync();
    }

    public class SessionHandler : ISessionHandler
    {
        public HttpContext Context { get; private set; }
        Task<WebSocket> getSocket;
        public WebSocket Socket { get; private set; }

        public SessionFactory Factory { get; private set; }

        public SessionHandler(HttpContext context, SessionFactory factory)
        {
            if (!context.IsWebSocketRequest)
                throw new ApplicationException("context is not a websocket context");

            this.Context = context;
            this.Factory = factory;

            this.getSocket = context.AcceptWebSocketAsync();
        }

        public async Task MessageLoopAsync()
        {
            var socket = await getSocket;

            var session = this.Factory.Create(socket);
            await MessageLoop(session);
            session.Close();
        }

        async Task MessageLoop(Session session)
        {
            try
            {
                // now loop for any incoming (we just echo)
                while (session.Socket.State == WebSocketState.Open)
                {
                    await session.ReceiveAsync();
                    if (session.Result.CloseStatus.HasValue)
                        break;

                    Message m = Message.Deserialize(new ArraySegment<byte>(session.Buffer, 0, session.Result.Count));
                    switch (m.Type)
                    {
                        case MessageType.Subscribe:
                            this.Factory.AddSubscription(session, (Subscribe) m);
                            break;
                        case MessageType.GetSubscriptions:
                            var list = this.Factory.GetSubscriptions(session);
                            var response = new Subscriptions();
                            response.List = list;
                            await session.SendAsync(response.Serialize());
                            break;
                        default:
                            await DoUnhandled(session, m);
                            break;
                    }
                }
            }
            catch { }
        }

        async Task DoUnhandled(Session session, Message m)
        {
            Error error = new Error("Unhandled message type: " + m.Type);
            await session.SendAsync(error.Serialize());
        }
    }
}