using Microsoft.Owin;
using Owin;
using VisualizationWeb.Helpers;

[assembly: OwinStartupAttribute(typeof(VisualizationWeb.Startup))]
namespace VisualizationWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            SingletonHolder.StartWebsocketServer();
            SingletonHolder.StartWebsocketClient();
        }
    }
}
