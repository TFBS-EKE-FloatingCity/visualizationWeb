using WebSocketSharp.Server;

namespace Application
{
   public class ApplicationWebSocketServer
   {
      private readonly WebSocketServer _webServer = new WebSocketServer("ws://localhost:8109");

      public void OpenConnection()
      {
         _webServer.AddWebSocketService<WebSocketServerConfiguration>("/Connection");
         _webServer.Start();
      }

      public void SendData(string json)
      {
         _webServer.WebSocketServices.BroadcastAsync(json, null);
      }
   }
}