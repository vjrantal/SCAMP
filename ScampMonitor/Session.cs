using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.WebSockets.Server;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor
{
    internal sealed class Session
    {
        public const int BufferSize = 2048;

        public Guid Id { get; private set; }

        public WebSocket Socket { get; private set; }

        public CancellationTokenSource Source { get; private set; }

        public byte[] Buffer { get; private set; }

        public WebSocketReceiveResult Result { get; private set; }

        public Session(WebSocket ws)
        {
            this.Id = Guid.NewGuid();
            this.Socket = ws;
            this.Source = new CancellationTokenSource();
            this.Buffer = new byte[BufferSize];
            this.Result = null;
        }

        public async Task ReceiveAsync()
        {
            var segment = new ArraySegment<byte>(this.Buffer);
            this.Result = await this.Socket.ReceiveAsync(segment, this.Source.Token);
        }

        public async Task SendAsync(byte[] message, int length)
        {
            var segment = new ArraySegment<byte>(message, 0, length);
            await this.Socket.SendAsync(segment, WebSocketMessageType.Binary, true, this.Source.Token);
        }

        public async Task SendAsync(byte[] message)
        {
            await SendAsync(message, message.Length);
        }

        public async Task EchoAsync()
        {
            try
            {
                // first thing for any new session we send our session id
                var sessionMessage = System.Text.UTF8Encoding.UTF8.GetBytes(this.Id.ToString());
                await this.SendAsync(sessionMessage, sessionMessage.Length);

                // now loop for any incoming (we just echo)
                while (this.Socket.State == WebSocketState.Open)
                {
                    await this.ReceiveAsync();
                    if (this.Result.CloseStatus.HasValue)
                        break;
                    await this.SendAsync(this.Buffer, this.Result.Count);
                }
            }
            catch { }
        }
    }
}