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
using ScampTypes.Messages;

namespace Monitor
{
    public sealed class Session
    {
        public const int BufferSize = 2048;

        public Guid Id { get; private set; }

        public WebSocket Socket { get; private set; }

        public CancellationTokenSource Source { get; private set; }

        public byte[] Buffer { get; private set; }

        ArraySegment<byte> receiver;

        public WebSocketReceiveResult Result { get; private set; }

        public SessionFactory Factory { get; private set; }

        public Session(SessionFactory factory, WebSocket ws)
        {
            this.Factory = factory;
            this.Id = Guid.NewGuid();
            this.Socket = ws;
            this.Source = new CancellationTokenSource();
            this.Buffer = new byte[BufferSize];
            receiver = new ArraySegment<byte>(this.Buffer);
            this.Result = null;
        }

        public async Task ReceiveAsync()
        {
            this.Result = await this.Socket.ReceiveAsync(receiver, this.Source.Token);
        }

        public async Task SendAsync(ArraySegment<byte> segment)
        {
            await this.Socket.SendAsync(segment, WebSocketMessageType.Binary, true, this.Source.Token);
        }

        public async Task SendAsync(byte[] message, int length)
        {
            var segment = new ArraySegment<byte>(message, 0, length);
            await SendAsync(segment);
        }

        public async Task SendAsync(byte[] message)
        {
            await SendAsync(message, message.Length);
        }

        public void Close()
        {
            this.Factory.Remove(this);
        }
    }
}