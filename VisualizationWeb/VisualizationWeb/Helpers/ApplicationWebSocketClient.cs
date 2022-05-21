using H.Socket.IO;
using H.Socket.IO.EventsArgs;
using Newtonsoft.Json;
using Simulation.Library;
using System;
using VisualizationWeb.Context;
using VisualizationWeb.Models;
using VisualizationWeb.ViewModel;

namespace VisualizationWeb.Helpers
{
   public class ApplicationWebSocketClient
   {
      public static SocketIoClient Client { get; private set; } = new SocketIoClient();

      private readonly ApplicationDbContext _context = new ApplicationDbContext();
      private readonly ApplicationWebSocketServer _server;
      private readonly SimulationService _service;
      private readonly Uri _uri;

      public ApplicationWebSocketClient(ApplicationWebSocketServer server, SimulationService simService, string connectionString)
      {
         if (connectionString != null) _uri = new Uri(connectionString);

         _server = server;
         _service = simService;
      }

      public void Connect()
      {
         if (_uri != null) Client.ConnectAsync(_uri);
      }

      public void Reconnect()
      {
         Client.DisconnectAsync();

         if (_uri != null) Client.ConnectAsync(_uri);
      }

      public void RegisterEvents()
      {
         Client.Connected += OnConnectedHandler;
         Client.On("sensorData", json => MessageHandler(json));
      }

      private void OnConnectedHandler(object sender, SocketIoEventEventArgs e)
      {
         Client.Emit("authenticate", "");
      }

      private void MessageHandler(string json)
      {
         DateTime recieved = DateTime.Now;

         JsonDataVM jsonData = JsonConvert.DeserializeObject<JsonDataVM>(json);
         JsonResponseVM response = new JsonResponseVM();

         if (_context.CityDatas.Find(jsonData.uuid) != null)
         {
            response.uuid = jsonData.uuid;
            response.energyBalance = _service.GetEnergyBalance(recieved);
            response.sun = _service.GetEnergyProductionSun(recieved);
            response.wind = _service.GetEnergyProductionWind(recieved);

            Client.Emit("sensorDataResponse", response);
         }

         CityData data = new CityData()
         {
            UUID = jsonData.uuid,
            CityDataHeadID = Mediator.CurrentCityDataHeadID
         };

         foreach (var module in jsonData.payload.modules)
         {
            switch (module.sector)
            {
               case "One":
                  data.USonicInner1 = ConvertToShort(module.sensorInside);
                  data.USonicOuter1 = ConvertToShort(module.sensorOutside);
                  data.Pump1 = ConvertToShort(module.pumpLevel);
                  break;

               case "Two":
                  data.USonicInner2 = ConvertToShort(module.sensorInside);
                  data.USonicOuter2 = ConvertToShort(module.sensorOutside);
                  data.Pump2 = ConvertToShort(module.pumpLevel);
                  break;

               case "Three":
                  data.USonicInner3 = ConvertToShort(module.sensorInside);
                  data.USonicOuter3 = ConvertToShort(module.sensorOutside);
                  data.Pump3 = ConvertToShort(module.pumpLevel);
                  break;

               default:
                  break;
            }
         }

         data.MesurementTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToDouble(jsonData.payload.timestamp));
         data.CreatedAt = recieved;

         data.WindMax = _service.MaxEnergyProductionWind;
         data.WindCurrent = Convert.ToInt16(_service.GetEnergyProductionWind(recieved));
         data.SunMax = _service.MaxEnergyProductionSun;
         data.SunCurrent = Convert.ToInt16(_service.GetEnergyProductionSun(recieved));
         data.ConsumptionMax = _service.MaxEnergyConsumption;
         data.ConsumptionCurrent = Convert.ToInt16(_service.GetEnergyConsumption(recieved));
         data.SimulationActive = _service.IsSimulationRunning;
         data.Simulationtime = _service.GetSimulatedTimeStamp(recieved);
         data.SimulationID = _service.SimulationScenarioId;
         data.TimeFactor = _service.TimeFactor;

         try
         {
            _context.CityDatas.Add(data);
            _context.SaveChanges();
            _server.SendData(JsonConvert.SerializeObject(data));

            response.status = "ack";
         }
         catch (Exception)
         {
            response.status = "error";
         }

         short ConvertToShort(int num)
         {
            if (num > short.MaxValue) return short.MaxValue;
            return Math.Max(Convert.ToInt16(num), (short)0);
         }
      }
   }
}