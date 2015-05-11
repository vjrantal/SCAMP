using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using ScampTypes.Messages;
using Newtonsoft.Json;

namespace MonitorTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        static async Task SendAsync(WebSocket ws, Message m)
        {
            using (MemoryStream mem = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(mem))
            {
                await sw.WriteAsync(JsonConvert.SerializeObject(m));
                await sw.FlushAsync();
                var seg = new ArraySegment<byte>(mem.GetBuffer(), 0, (int)mem.Length);
                await ws.SendAsync(seg, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static async Task<Message> ReceiveAsync(WebSocket ws)
        {
            byte[] buffer = new byte[4096];
            var seg = new ArraySegment<byte>(buffer);
            var result = await ws.ReceiveAsync(seg, CancellationToken.None);
            return Message.Deserialize(new ArraySegment<byte>(buffer, 0, result.Count));
        }

        static async Task Run()
        {
            var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri("ws://localhost:8088"), CancellationToken.None);

            var sub1 = new Subscribe() { User = "abc" };
            await SendAsync(ws, sub1);

            var sub2 = new Subscribe() { User = "def" };
            await SendAsync(ws, sub2);

            var req = new GetSubscriptions();
            await SendAsync(ws, req);

            var msg = await ReceiveAsync(ws);
            var subs = (Subscriptions)msg;

            foreach (var s in subs.List)
            {
                Console.WriteLine("Sub user='" + s + "'");
            }
        }

    }
}
