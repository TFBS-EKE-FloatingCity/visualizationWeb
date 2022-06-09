using Application;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(VisualizationWeb.Startup))]

namespace VisualizationWeb
{
   public partial class Startup
   {
      public void Configuration(IAppBuilder app)
      {
         ConfigureAuth(app);

         Mediator.StartWebsocketServer();
         Mediator.StartWebsocketClient();
      }
   }
}