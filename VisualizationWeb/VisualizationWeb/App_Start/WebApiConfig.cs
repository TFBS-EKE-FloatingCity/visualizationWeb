using System.Web.Http;

namespace UI
{
   public static class WebApiConfig
   {
      public static void Register(HttpConfiguration config)
      {
         config.MapHttpAttributeRoutes();
      }
   }
}