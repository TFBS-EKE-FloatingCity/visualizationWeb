using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSocketSharp;

namespace VisualizationWeb.Helpers {
    public class WebSocketClient {
        WebSocket socket = new WebSocket("ws://161.97.116.72:8080/socket.io/?EIO=2&transport=websocket");
        //WebSocket socket = new WebSocket("ws://161.97.116.72:8889/ping");
    
        public void Connect() {
            socket.Connect();
        }

        public void RegisterEvents() {
            socket.OnMessage += MessageHandler;
            socket.OnError += ErrorHandler;
        }


        private void MessageHandler(object sender, object e) {
            var test = sender;
        }
        
        public void SendMessage(string message) {
            socket.Send(message);
        }


        private void ErrorHandler(object sender, object e) {
            var test = e;
        }
    }
}