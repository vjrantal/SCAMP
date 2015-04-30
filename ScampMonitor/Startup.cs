using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.WebSockets.Server;
using Microsoft.AspNet.WebSockets.Protocol;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Monitor
{
    public class Startup
    {
        ConcurrentDictionary<Guid, Session> sessions = new ConcurrentDictionary<Guid, Session>();

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Map("", m =>
            {
                m.UseWebSockets(new WebSocketOptions() { ReplaceFeature = true });
                m.Use(WebSocketHandler);
            });
        }

        async Task WebSocketHandler(HttpContext context, Func<Task> next)
        {
            if (!context.IsWebSocketRequest)
            {
                await next();
                return;
            }

            var socket = await context.AcceptWebSocketAsync();
            var session = new Session(socket);
            sessions[session.Id] = session;
            await session.EchoAsync();
            sessions.TryRemove(session.Id, out session);
        }
    }
}
