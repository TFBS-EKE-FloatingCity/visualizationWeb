using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VisualizationWeb.Startup))]
namespace VisualizationWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
