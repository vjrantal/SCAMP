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
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketHandler();
        }

        public void Configure(IApplicationBuilder app)
        {

            //app.Map("", (a) =>
            //{
            //    a.UseWebSockets(new WebSocketOptions() { ReplaceFeature = true });
            //    a.Use<IWebSocketHandler>(async (context, next, handler) =>
            //    {
            //        if (!context.IsWebSocketRequest)
            //        {
            //            await next();
            //            return;
            //        }

            //        await handler.MessageLoopAsync();
            //    });
            //});

            app.Map("", (a) =>
            {
                a.UseWebSockets(new WebSocketOptions() { ReplaceFeature = true });
                a.Use(async (context, next) =>
                {
                    if (!context.IsWebSocketRequest)
                    {
                        await next();
                        return;
                    }

                    //                    var handler = a.ApplicationServices.GetRequiredService<ISessionHandler>();
                    var factory = a.ApplicationServices.GetService<SessionFactory>();
                    var handler = new SessionHandler(context, factory);
                    await handler.MessageLoopAsync();
                });
            });
        }
    }
}
