using System.Web.Optimization;

namespace VisualizationWeb
{
   public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles( BundleCollection bundles ) {
            bundles.Add( new ScriptBundle( "~/bundles/jquery" ).Include(
                        "~/Scripts/jquery-{version}.js" ) );

            bundles.Add( new ScriptBundle( "~/bundles/jqueryval" ).Include(
                        "~/Scripts/jquery.validate*" ) );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add( new ScriptBundle( "~/bundles/modernizr" ).Include(
                        "~/Scripts/modernizr-*" ) );

            bundles.Add( new ScriptBundle( "~/bundles/bootstrap" ).Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/init.js") );

            bundles.Add( new StyleBundle( "~/Content/css" ).Include(
                      "~/Content/bootstrap/bootstrap.css",
                      "~/Content/general/buttons.css",
                      "~/Content/general/formelements.css",
                      "~/Content/account/login.css",
                      "~/Content/account/register.css",
                      "~/Content/navigation.css",
                      "~/Content/actionsbar.css",
                      "~/Content/sensors.css",
                      "~/Content/dashboard.css",
                      "~/Content/settings.css" ));

            bundles.Add(new StyleBundle("~/Content/layouts/css").Include(
                      "~/Content/layouts/applayout.css",
                      "~/Content/layouts/emptylayout.css"));
        }
    }
}
