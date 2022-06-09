using Application.WebSockets;
using WebSocketSharp.Server;

namespace Application.Websockets
{
   public class ApplicationWebSocketServer
   {
      private readonly WebSocketServer _webServer = new WebSocketServer("ws://localhost:8109");

      public void OpenConnection()
      {
         _webServer.AddWebSocketService<WebsocketServerBehavior>("/Connection");
         _webServer.Start();
      }

      public void SendData(string json)
      {
         _webServer.WebSocketServices.BroadcastAsync(json, null);
      }
   }
}