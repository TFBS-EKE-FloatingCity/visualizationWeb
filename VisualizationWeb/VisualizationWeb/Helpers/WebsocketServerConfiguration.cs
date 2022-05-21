using WebSocketSharp;
using WebSocketSharp.Server;

namespace VisualizationWeb.Helpers
{
   public class WebSocketServerConfiguration : WebSocketBehavior
   {
      protected override void OnOpen()
      {
         base.OnOpen();
         Mediator.SendCityDataHead();
      }

      // @Sascha: I assume these were defined for programmatic detection. 
      // shouldnt be necessary, but not sure, so dont remove. 
      protected override void OnClose(CloseEventArgs e) => base.OnClose(e);

      protected override void OnMessage(MessageEventArgs e) => base.OnMessage(e);

      protected override void OnError(ErrorEventArgs e) => base.OnError(e);
   }
}