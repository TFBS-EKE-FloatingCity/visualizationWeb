using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisualizationWeb.Helpers.Temporary;

namespace VisualizationWeb.Helpers {
    public static class SingletonHolder {
        private static readonly SimulationService simService = new SimulationService();
        private static readonly WebsocketServer server = new WebsocketServer();
        private static readonly WebSocketClient client = new WebSocketClient(server, simService);

        public static void StartWebsocketClient() {
            client.RegisterEvents();
            client.Connect();
        }

        public static void StartWebsocketServer() {
            server.OpenConnection();
        }

        public static WebSocketClient Client {
            get {
                return client;
            }
        }
    }
}