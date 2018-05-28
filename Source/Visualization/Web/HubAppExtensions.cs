using Microsoft.AspNetCore.Builder;

namespace Web
{  
    public static class HubAppExtensions
    {      
        public static IApplicationBuilder UseHub<THub>(this IApplicationBuilder app, string route) where THub : Hub
        {
            app.Use(async(context, next) =>
            {
                if (context.Request.Path == route )
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var hubs = app.ApplicationServices.GetService(typeof(IHubs)) as IHubs;
                        var hub = hubs.Get<THub>();
                        await hub.Run(context, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });

            return app;
        }
    }
}