using Application.Services;
using Core;
using DataAccess;
using DataAccess.Entities;
using H.Socket.IO;
using H.Socket.IO.EventsArgs;
using Newtonsoft.Json;
using System;

namespace Application.Websockets
{
   public class SocketClient
   {
      public static SocketIoClient Client { get; private set; } = new SocketIoClient();

      private readonly Context _context = new Context();
      private readonly SocketServer _server;
      private readonly SimulationService _service;
      private readonly Uri _uri;

      public SocketClient(SocketServer server, SimulationService simService, string connectionString)
      {
         if (connectionString != null) _uri = new Uri(connectionString);

         _server = server ?? throw new ArgumentNullException(nameof(server));
         _service = simService ?? throw new ArgumentNullException(nameof(simService));
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

         JsonData jsonData = JsonConvert.DeserializeObject<JsonData>(json);
         JsonResponse response = new JsonResponse();

         CityData data = new CityData()
         {
            UUID = jsonData.uuid,
            CityDataHeadID = Mediator.CurrentCityDataHeadID
         };

         foreach (var module in jsonData.payload.modules)
         {
            switch (module.sector)
            {
               case Module.Sectors.One:
                  data.USonicInner1 = Int32ToShort(module.sensorInside);
                  data.USonicOuter1 = Int32ToShort(module.sensorOutside);
                  data.Pump1 = Int32ToShort(module.pumpLevel);
                  break;

               case Module.Sectors.Two:
                  data.USonicInner2 = Int32ToShort(module.sensorInside);
                  data.USonicOuter2 = Int32ToShort(module.sensorOutside);
                  data.Pump2 = Int32ToShort(module.pumpLevel);
                  break;

               case Module.Sectors.Three:
                  data.USonicInner3 = Int32ToShort(module.sensorInside);
                  data.USonicOuter3 = Int32ToShort(module.sensorOutside);
                  data.Pump3 = Int32ToShort(module.pumpLevel);
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
            if (_context.CityDatas.Find(data.UUID) == null)
            {
               _context.CityDatas.Add(data);
               _context.SaveChanges();
            }
            _server.SendData(JsonConvert.SerializeObject(data));

            response.status = "ack";
         }
         catch (Exception)
         {
            response.status = "error";
         }

         if (_context.CityDatas.Find(jsonData.uuid) != null)
         {
            response.uuid = jsonData.uuid;
            response.energyBalance = _service.GetEnergyBalance(recieved);
            response.sun = _service.GetEnergyProductionSun(recieved);
            response.wind = _service.GetEnergyProductionWind(recieved);

            Client.Emit("sensorDataResponse", response);
         }
      }

      /// <summary>
      /// Converts an Int32 to Short, capping the value at the minimum and maximum values.
      /// </summary>
      short Int32ToShort(int num)
      {
         if (num > short.MaxValue) return short.MaxValue;
         return Math.Max(Convert.ToInt16(num), short.MinValue);
      }
   }
}