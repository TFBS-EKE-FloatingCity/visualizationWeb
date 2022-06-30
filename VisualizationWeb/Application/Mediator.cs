using Application.Services;
using Application.Websockets;
using Core.Entities;
using DataAccess;
using DataAccess.Repositories;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Application
{
   public static class Mediator
   {
      public static int? CurrentCityDataHeadID => CurrentCityDataHead?.CityDataHeadID;

      public static CityDataHead CurrentCityDataHead { get; private set; }

      private static Context _context = new Context();

      private static Setting _settings = new SettingsRepository().GetSimulationSettings();


      private static readonly SimulationService _simService = new SimulationService();


      private static readonly SocketServer _server = new SocketServer();
   
      private static readonly SocketClient _wsClient = new SocketClient(_server, _simService, _settings.rbPiConnectionString);

      public static void UpdateSimulationSettings(Setting setting)
      {
         _simService.SetSettings(setting);
         _settings = setting;
      }

      /// <exception cref="Core.Exceptions.InvalidDurationException"/>
      /// <exception cref="Core.Exceptions.InvalidScenarioException"/>
      public static void StartSimulation(SimScenario simScenario, TimeSpan duration)
      {
         if (_simService.IsSimulationRunning) return;

         _simService.Run(simScenario, duration);

         _simService.SimulationEnded += HandleStopSimulationEvent;

         CityDataHead head = new CityDataHead
         {
            StartTime = (DateTime)_simService.StartDateTimeReal,
            EndTime = (DateTime)_simService.EndDateTimeReal,
            SimulationID = simScenario.SimScenarioID,
            State = "Running"
         };

         _context.CityDataHeads.Add(head);
         _context.SaveChanges();

         head.CityDataHeadID = (from cdh in _context.CityDataHeads
                                orderby cdh.CityDataHeadID descending
                                select cdh.CityDataHeadID).FirstOrDefault();

         CurrentCityDataHead = head;
         //CurrentCityDataHeadID = head.CityDataHeadID;

         SendCityDataHead();
      }

      public static void SendCityDataHead()
      {
         if (CurrentCityDataHead != null)
         {
            _server.SendData(JsonConvert.SerializeObject(CurrentCityDataHead));
         }
      }

      public static void StopSimulation()
      {
         if (!_simService.IsSimulationRunning) return;

         _simService.Stop();

         CityDataHead head = _context.CityDataHeads.Find(CurrentCityDataHeadID);

         head.EndTime = DateTime.Now;
         head.State = "Stopped";

         _context.SaveChanges();

         CurrentCityDataHead = null;
         //CurrentCityDataHeadID = null;

         _server.SendData(JsonConvert.SerializeObject(head));
      }

      public static void SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind)
      {
         _simService.SetIdleValues(energyConsumption, energyProductionSun, energyProductionWind);
      }

      public static void StartWebsocketClient()
      {
         _wsClient.RegisterEvents();
         _wsClient.Connect();
      }

      public static void RestartWebsocketClient()
      {
         _wsClient.Reconnect();
      }

      public static void StartWebsocketServer()
      {
         _server.OpenConnection();
      }


      private static void HandleStopSimulationEvent(object sender, EventArgs a)
      {
         _simService.SimulationEnded -= HandleStopSimulationEvent;
         StopSimulation();
      }
   }
}