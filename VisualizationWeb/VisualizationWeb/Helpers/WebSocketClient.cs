using H.Socket.IO;
using H.Socket.IO.EventsArgs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using VisualizationWeb.Models;
using VisualizationWeb.Models.ViewModel;
using WebSocketSharp;

namespace VisualizationWeb.Helpers {
    public class WebSocketClient {

        private ApplicationDbContext db = new ApplicationDbContext();

        WebsocketServer websocketserver;
        SimulationService simService;

        Uri url = new Uri(ConfigurationManager.AppSettings["RaspberryWebsocketConnectionString"]);
        public static SocketIoClient client = new SocketIoClient();

        public WebSocketClient(WebsocketServer server, SimulationService simService) {
            this.websocketserver = server;
            this.simService = simService;
        }

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

            DateTime recieved = DateTime.Now;

            JsonDataVM jsonData = JsonConvert.DeserializeObject<JsonDataVM>(json);
            JsonResponseVM response = new JsonResponseVM();

            if (db.CityDatas.Find(jsonData.uuid) == null) {

                CityData data = new CityData();

                data.UUID = jsonData.uuid;

                foreach (var module in jsonData.payload.modules) {
                    if (module.sector == "One") {
                        data.USonicInner1 = Convert.ToInt16(module.sensorInside);
                        data.USonicOuter1 = Convert.ToInt16(module.sensorOutside);
                        data.Pump1 = Convert.ToInt16(module.pumpLevel);
                    }
                    else if (module.sector == "Two") {
                        data.USonicInner2 = Convert.ToInt16(module.sensorInside);
                        data.USonicOuter2 = Convert.ToInt16(module.sensorOutside);
                        data.Pump2 = Convert.ToInt16(module.pumpLevel);
                    }
                    else if (module.sector == "Three") {
                        data.USonicInner3 = Convert.ToInt16(module.sensorInside);
                        data.USonicOuter3 = Convert.ToInt16(module.sensorOutside);
                        data.Pump3 = Convert.ToInt16(module.pumpLevel);
                    }
                }

                data.MesurementTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(Convert.ToDouble(jsonData.payload.timestamp));
                data.CreatedAt = recieved;

                data.WindMax = simService.MaxEnergyProductionWind;
                data.WindCurrent = Convert.ToInt16(simService.GetEnergyProductionWind(recieved));
                data.SunMax = simService.MaxEnergyProductionSun;
                data.SunCurrent = Convert.ToInt16(simService.GetEnergyProductionSun(recieved));
                data.ConsumptionMax = simService.MaxEnergyConsumption;
                data.ConsumptionCurrent = Convert.ToInt16(simService.GetEnergyConsumption(recieved));
                data.SimulationActive = simService.IsSimulationRunning;
                data.Simulationtime = simService.GetSimulatedTimeStamp(recieved);
                data.SimulationID = simService.SimulationScenarioId;
                data.TimeFactor = simService.TimeFactor;

                try {
                    db.CityDatas.Add(data);
                    db.SaveChanges();
                    websocketserver.SendData(JsonConvert.SerializeObject(data));

                    response.status = "ack";
                }
                catch (Exception e) {
                    response.status = "error";
                }

            }

            response.uuid = jsonData.uuid;
            response.energyBalance = simService.GetEnergyBalance(recieved);
            response.sun = simService.GetEnergyProductionSun(recieved);
            response.wind = simService.GetEnergyProductionWind(recieved);
            
            client.Emit("sensorDataResponse", response);
        }
    }
}