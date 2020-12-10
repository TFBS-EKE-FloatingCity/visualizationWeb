using H.Socket.IO;
using H.Socket.IO.EventsArgs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VisualizationWeb.Models;
using WebSocketSharp;

namespace VisualizationWeb.Helpers {
    public class WebSocketClient {

        private ApplicationDbContext db = new ApplicationDbContext();

        Uri url = new Uri("ws://161.97.116.72:8080");
        public static SocketIoClient client = new SocketIoClient();

        public void Connect() {
            client.ConnectAsync(url);
        }

        public void RegisterEvents() {
            client.Connected += onConnected;

            client.On("sensorData", json => {
                MessageHandler(json);
            });
        }

        private void onConnected(object sender, SocketIoEventEventArgs e) {
            client.Emit("authenticate", "");
        }

        private void MessageHandler(string json) {

            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);
            DataTable dataTable = dataSet.Tables["modules"];

            CityData data = new CityData();

            foreach(DataRow row in dataTable.Rows) {

                if (row["sector"].ToString() == "One") {
                    data.USonicInner1 = Convert.ToInt32(row["sensorInside"]);
                    data.USonicOuter1 = Convert.ToInt32(row["sensorOutside"]);
                    data.Pump1 = Convert.ToInt32(row["pumpLevel"]);
                }
                else if (row["sector"].ToString() == "Two") {
                    data.USonicInner2 = Convert.ToInt32(row["sensorInside"]);
                    data.USonicOuter2 = Convert.ToInt32(row["sensorOutside"]);
                    data.Pump2 = Convert.ToInt32(row["pumpLevel"]);
                }
                else if (row["sector"].ToString() == "Three") {
                    data.USonicInner3 = Convert.ToInt32(row["sensorInside"]);
                    data.USonicOuter3 = Convert.ToInt32(row["sensorOutside"]);
                    data.Pump3 = Convert.ToInt32(row["pumpLevel"]);
                }
            }

            data.CreatedAt = DateTime.Now;

            db.CityDatas.Add(data);
            db.SaveChanges();


        }
    }
}