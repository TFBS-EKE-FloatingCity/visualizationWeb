using WebSocketSharp.Server;

namespace VisualizationWeb.Helpers
{
   public class WebsocketServer 
    {
        private readonly WebSocketServer _webServer = new WebSocketServer("ws://localhost:8109");

        public void OpenConnection()
        {
            _webServer.AddWebSocketService<WebsocketServerConfiguration>("/Connection");
            _webServer.Start();
        }

        public void SendData(string json) 
        {
            _webServer.WebSocketServices.BroadcastAsync(json, null);
        }
    }
}