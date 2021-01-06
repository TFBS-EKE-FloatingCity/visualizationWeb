using H.Socket.IO;
using H.Socket.IO.EventsArgs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VisualizationWeb.Helpers.Temporary;
using VisualizationWeb.Models;
using VisualizationWeb.Models.ViewModel;
using WebSocketSharp;

namespace VisualizationWeb.Helpers {
    public class WebSocketClient {

        private ApplicationDbContext db = new ApplicationDbContext();

        WebsocketServer websocketserver;

        Uri url = new Uri("ws://161.97.116.72:8080");
        public static SocketIoClient client = new SocketIoClient();

        SimulationService simService;

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

            JsonDataVM jsonData = JsonConvert.DeserializeObject<JsonDataVM>(json);

            CityData data = new CityData();

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
            data.CreatedAt = DateTime.Now;

            data.WindMax = simService.MaxEnergyProductionWind ;
            data.WindCurrent = Convert.ToInt16(simService.GetEnergyProductionWind(data.CreatedAt));
            data.SunMax = simService.MaxEnergyProductionSun;
            data.SunCurrent = Convert.ToInt16(simService.GetEnergyProductionSun(data.CreatedAt));
            data.ConsumptionMax = simService.MaxEnergyConsumption;
            data.ConsumptionCurrent = Convert.ToInt16(simService.GetEnergyConsumption(data.CreatedAt));
            data.SimulationActive = simService.IsSimulationRunning;
            data.Simulationtime = simService.GetSimulatedTimeStamp(data.CreatedAt);
            data.SimulationID = simService.SimulationScenarioId;
            data.TimeFactor = simService.TimeFactor;

            JsonResponseVM response = new JsonResponseVM();
            
            try {
                db.CityDatas.Add(data);
                db.SaveChanges();
                response.status = "ack";
            }
            catch (Exception) {
                response.status = "error";
            }

            response.uuid = jsonData.uuid;
            response.energyBalance = simService.GetEnergyBalance(data.CreatedAt);
            response.sun = simService.GetEnergyProductionSun(data.CreatedAt);
            response.wind = simService.GetEnergyProductionWind(data.CreatedAt);
            
            client.Emit("sensorDataResponse", response);

            websocketserver.SendData(JsonConvert.SerializeObject(data));
        }
    }
}