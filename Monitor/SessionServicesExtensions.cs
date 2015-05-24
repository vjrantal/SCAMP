using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.DependencyInjection;

namespace Monitor
{
    public static class WebSocketServicesExtensions
    {
        public static void AddWebSocketHandler(this IServiceCollection services)
        {
            services.AddInstance<SessionFactory>(new SessionFactory());
            services.AddTransient<ISessionHandler, SessionHandler>();
        }
    }
}
