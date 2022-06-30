using Application;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(UI.Startup))]

namespace UI
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