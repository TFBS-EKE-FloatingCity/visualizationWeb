using Application.WebSockets;
using WebSocketSharp.Server;

namespace Application.Websockets
{
   public class SocketServer
   {
      private readonly WebSocketServer _webServer = new WebSocketServer("ws://localhost:8109");

      public void OpenConnection()
      {
         _webServer.AddWebSocketService<SocketBehavior>("/Connection");
         _webServer.Start();
      }

      public void SendData(string json)
      {
         _webServer.WebSocketServices.BroadcastAsync(json, null);
      }
   }
}