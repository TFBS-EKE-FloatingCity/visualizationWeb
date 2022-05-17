using WebSocketSharp.Server;

namespace VisualizationWeb.Helpers
{
   public class WebsocketServer 
    {
        WebSocketServer webServer = new WebSocketServer("ws://localhost:8109");

        public void OpenConnection()
        {
            webServer.AddWebSocketService<WebsocketServerConfiguration>("/Connection");

            webServer.Start();

        }

        public void SendData(string json) 
        {
            webServer.WebSocketServices.BroadcastAsync(json, null);
        }
    }
}