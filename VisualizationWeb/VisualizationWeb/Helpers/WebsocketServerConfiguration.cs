using WebSocketSharp;
using WebSocketSharp.Server;

namespace VisualizationWeb.Helpers
{
   public class WebsocketServerConfiguration : WebSocketBehavior {
        protected override void OnOpen() {
            base.OnOpen();
            SingletonHolder.SendCityDataHead();
        }

        protected override void OnClose(CloseEventArgs e) {
            base.OnClose(e);
        }

        protected override void OnMessage(MessageEventArgs e) {
            base.OnMessage(e);
        }

        protected override void OnError(ErrorEventArgs e) {
            base.OnError(e);
        }
    }
}