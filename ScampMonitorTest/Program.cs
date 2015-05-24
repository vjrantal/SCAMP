using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace ScampMonitorTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        static async Task Run()
        {
            var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri("ws://localhost:8088"), CancellationToken.None);

            
            byte[] buf = new byte[4096];
            var seg = new ArraySegment<byte>(buf, 0, buf.Length);
            var r1 = await ws.ReceiveAsync(seg, CancellationToken.None);
            string id = UTF8Encoding.UTF8.GetString(buf, 0, r1.Count);
            Console.WriteLine("SessionId=" + id);

            for (int i = 0; i < 20; i++)
            {
                var msg = UTF8Encoding.UTF8.GetBytes("Send(" + i + ")");
                await ws.SendAsync(new ArraySegment<byte>(msg, 0, msg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                var rcv = await ws.ReceiveAsync(seg, CancellationToken.None);
                string response = UTF8Encoding.UTF8.GetString(buf, 0, rcv.Count);
                Console.WriteLine("Response=" + response);
                Thread.Sleep(2000);
            }
        }
    }
}
