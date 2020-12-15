using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Helpers {
    public class SingletonHolder {
        private static readonly WebsocketServer server = new WebsocketServer();
        private static readonly WebSocketClient client = new WebSocketClient(server);

        public SingletonHolder() {
            client.RegisterEvents();
            client.Connect();

            server.OpenConnection();
        }

        public static WebSocketClient Client {
            get {
                return client;
            }
        }
    }
}