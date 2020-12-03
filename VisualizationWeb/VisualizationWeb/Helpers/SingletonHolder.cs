using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Helpers {
    public class SingletonHolder {
        private static readonly WebSocketClient client = new WebSocketClient();

        public SingletonHolder() {
            client.RegisterEvents();
            client.Connect();
        }

        public static WebSocketClient Client {
            get {
                return client;
            }
        }
    }
}