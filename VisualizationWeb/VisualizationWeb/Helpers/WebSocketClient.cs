using H.Socket.IO;
using H.Socket.IO.EventsArgs;
using Newtonsoft.Json;
using Simulation.Library.Models;
using System;
using VisualizationWeb.Context;
using VisualizationWeb.Models;
using VisualizationWeb.Models.ViewModel;

namespace VisualizationWeb.Helpers
{
   public class WebSocketClient
   {

      private ApplicationDbContext db = new ApplicationDbContext();

      WebsocketServer websocketserver;
      SimulationService simService;

      Uri url;
      public static SocketIoClient client = new SocketIoClient();

      public WebSocketClient(WebsocketServer server, SimulationService simService, string connectionString)
      {
         this.websocketserver = server;
         this.simService = simService;

         if (connectionString != null)
         {
            url = new Uri(connectionString);
         }
      }

      public void Connect()
      {
         if (url != null)
         {
            client.ConnectAsync(url);
         }
      }

      public void ReConnect()
      {
         client.DisconnectAsync();

         if (url != null)
         {
            client.ConnectAsync(url);
         }
      }

      public void RegisterEvents()
      {
         client.Connected += onConnected;

         client.On("sensorData", json =>
         {
            MessageHandler(json);
         });
      }

      private void onConnected(object sender, SocketIoEventEventArgs e)
      {
         client.Emit("authenticate", "");
      }

      private void MessageHandler(string json)
      {

         DateTime recieved = DateTime.Now;

         JsonDataVM jsonData = JsonConvert.DeserializeObject<JsonDataVM>(json);
         JsonResponseVM response = new JsonResponseVM();

         if (db.CityDatas.Find(jsonData.uuid) == null)
         {

            CityData data = new CityData();

            data.UUID = jsonData.uuid;
            data.CityDataHeadID = SingletonHolder.CurrentCityDataHeadID;

            foreach (var module in jsonData.payload.modules)
            {
               switch (module.sector)
               {
                  case "One":
                     data.USonicInner1 = Convert.ToInt16(module.sensorInside);
                     data.USonicOuter1 = Convert.ToInt16(module.sensorOutside);
                     data.Pump1 = Convert.ToInt16(module.pumpLevel);
                     break;
                  case "Two":
                     data.USonicInner2 = Convert.ToInt16(module.sensorInside);
                     data.USonicOuter2 = Convert.ToInt16(module.sensorOutside);
                     data.Pump2 = Convert.ToInt16(module.pumpLevel);
                     break;
                  case "Three":
                     data.USonicInner3 = Convert.ToInt16(module.sensorInside);
                     data.USonicOuter3 = Convert.ToInt16(module.sensorOutside);
                     data.Pump3 = Convert.ToInt16(module.pumpLevel);
                     break;
                  default:
                     break;
               }
            }

            data.MesurementTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToDouble(jsonData.payload.timestamp));
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

            try
            {
               db.CityDatas.Add(data);
               db.SaveChanges();
               websocketserver.SendData(JsonConvert.SerializeObject(data));

               response.status = "ack";
            }
            catch (Exception)
            {
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