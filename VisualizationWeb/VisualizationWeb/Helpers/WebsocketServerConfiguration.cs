using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace VisualizationWeb.Helpers {
    public class WebsocketServerConfiguration : WebSocketBehavior {
        protected override void OnOpen() {
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e) {
            base.OnClose(e);
        }

        protected override void OnMessage(MessageEventArgs e) {
            Sessions.Broadcast(e.Data);
        }

        protected override void OnError(ErrorEventArgs e) {
            base.OnError(e);
        }
    }
}