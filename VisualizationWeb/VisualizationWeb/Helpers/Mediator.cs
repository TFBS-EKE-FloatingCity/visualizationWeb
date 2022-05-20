using Newtonsoft.Json;
using Simulation.Library;
using Simulation.Library.Models;
using System;
using System.Linq;
using VisualizationWeb.Context;
using VisualizationWeb.Models;
using VisualizationWeb.Models.Repository;

namespace VisualizationWeb.Helpers
{
   public static class Mediator
   {
      /// <summary>
      ///   Die ID des Kopfdatensatzes der aktuell laufenden Simulation
      /// </summary>
      public static int? CurrentCityDataHeadID { get; private set; }

      /// <summary>
      ///   der Kopfdatensatz der aktuell laufenden Simulation
      /// </summary>
      public static CityDataHead CurrentCityDataHead { get; private set; }

      /// <summary>
      ///   Instanz des SimulationService welcher von Max Hiltpolt geschrieben wurde An diesen
      ///   werden die Settings übergeben
      /// </summary>
      private static readonly SimulationService _simService = new SimulationService(_settings);

      /// <summary>
      ///   Websocketserver mit welchem sich der Browser verbindet um Live Daten zu holen
      /// </summary>
      private static readonly ApplicationWebSocketServer _server = new ApplicationWebSocketServer();

      /// <summary>
      ///   Websocketclient welcher sich mit dem Raspberry verbindet um die Live Daten zu bekommen,
      ///   abzuspeichern und über den Websocketserver an die Browser zu verteilen
      /// </summary>
      private static readonly ApplicationWebSocketClient _wsClient = new ApplicationWebSocketClient(_server, _simService, _settings.rbPiConnectionString);

      private static ApplicationDbContext _db = new ApplicationDbContext();

      /// <summary>
      ///   Aus dem Simulationrepository werden hier die Settings geholt und abgespeichert
      /// </summary>
      private static SimulationRepository _repo = new SimulationRepository(_db);

      private static Setting _settings = _repo.GetSimulationSetting();

      /// <summary>
      ///   Wenn auf der Settings unterseite Settings geändert werden werden diese auch hier im
      ///   Simulationservice geändert ohne einen Webserver neustart
      /// </summary>
      /// <param name="setting"> </param>
      public static void UpdateSimulationSettings(Setting setting)
      {
         _simService.SetSettings(setting);
         _settings = setting;
      }

      /// <summary>
      ///   Wird aufgerufen wenn eine Simulation über das Dashboard von einem angemeldeten User
      ///   gestartet wird
      /// </summary>
      /// <param name="simScenario"> </param>
      /// <param name="duration"> </param>
      public static void StartSimulation(SimScenario simScenario, TimeSpan duration)
      {
         _simService.Run(simScenario, duration);

         _simService.SimulationEnded += HandleStopSimulationEvent;

         CityDataHead head = new CityDataHead
         {
            StartTime = (DateTime)_simService.StartDateTimeReal,
            EndTime = (DateTime)_simService.EndDateTimeReal,
            SimulationID = simScenario.SimScenarioID,
            State = "Running"
         };

         _db.CityDataHeads.Add(head);
         _db.SaveChanges();

         head.CityDataHeadID = (from cdh in _db.CityDataHeads
                                orderby cdh.CityDataHeadID descending
                                select cdh.CityDataHeadID).FirstOrDefault();

         CurrentCityDataHeadID = head.CityDataHeadID;
         CurrentCityDataHead = head;

         SendCityDataHead();
      }

      /// <summary>
      ///   Senden der aktuellen Kopfdaten an die Brwoser via dem Websocketserver
      /// </summary>
      public static void SendCityDataHead()
      {
         if (CurrentCityDataHead != null)
         {
            _server.SendData(JsonConvert.SerializeObject(CurrentCityDataHead));
         }
      }

      /// <summary>
      ///   Wenn eine Simulation manuell frühzeitig gestoppt wird
      /// </summary>
      public static void StopSimulation()
      {
         _simService.Stop();

         CityDataHead head = _db.CityDataHeads.Find(CurrentCityDataHeadID);

         head.EndTime = DateTime.Now;
         head.State = "Stopped";

         _db.SaveChanges();

         CurrentCityDataHead = null;
         CurrentCityDataHeadID = null;

         _server.SendData(JsonConvert.SerializeObject(head));
      }

      /// <summary>
      ///   Setzen der Idle Values
      /// </summary>
      /// <param name="energyConsumption"> </param>
      /// <param name="energyProductionSun"> </param>
      /// <param name="energyProductionWind"> </param>
      public static void SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind)
      {
         _simService.SetIdleValues(energyConsumption, energyProductionSun, energyProductionWind);
      }

      /// <summary>
      ///   Starten des Websocketclients und Verbindungsaufbau mit dem Raspberry
      /// </summary>
      public static void StartWebsocketClient()
      {
         _wsClient.RegisterEvents();
         _wsClient.Connect();
      }

      /// <summary>
      ///   Falls der Connectionstring geändert wird kann hier der Websocketclient neugestartet werden
      /// </summary>
      public static void RestartWebsocketClient()
      {
         _wsClient.Reconnect();
      }

      /// <summary>
      ///   Start des Websocketservers damit sich Browser damit verbinden können
      /// </summary>
      public static void StartWebsocketServer()
      {
         _server.OpenConnection();
      }

      /// <summary>
      ///   Event wenn eine Simulation zuende gelaufen ist
      /// </summary>
      /// <param name="sender"> </param>
      /// <param name="a"> </param>
      private static void HandleStopSimulationEvent(object sender, System.EventArgs a)
      {
         _simService.SimulationEnded -= HandleStopSimulationEvent;

         StopSimulation();
      }
   }
}