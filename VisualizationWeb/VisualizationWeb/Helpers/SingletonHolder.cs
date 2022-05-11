using Newtonsoft.Json;
using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisualizationWeb.Context;
using VisualizationWeb.Models;
using VisualizationWeb.Models.Repo;

namespace VisualizationWeb.Helpers {
    public static class SingletonHolder {

        private static ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Aus dem Simulationrepository werden hier die Settings geholt und abgespeichert
        /// </summary>
        private static SimulationRepository repo = new SimulationRepository(db);
        private static Setting settings = repo.GetSimulationSetting();

        /// <summary>
        /// Instanz des SimulationService welcher von Max Hiltpolt geschrieben wurde
        /// An diesen werden die Settings übergeben
        /// </summary>
        private static readonly SimulationService simService = new SimulationService(settings);

        /// <summary>
        /// Websocketserver mit welchem sich der Browser verbindet um Live Daten zu holen
        /// </summary>
        private static readonly WebsocketServer server = new WebsocketServer();

        /// <summary>
        /// Websocketclient welcher sich mit dem Raspberry verbindet um die Live Daten zu bekommen, abzuspeichern und über den Websocketserver an die Browser zu verteilen
        /// </summary>
        private static readonly WebSocketClient client = new WebSocketClient(server, simService, settings.rbPiConnectionString);

        /// <summary>
        /// Die ID des Kopfdatensatzes der aktuell laufenden Simulation
        /// </summary>
        private static int? currentCityDataHeadID;
        public static int? CurrentCityDataHeadID {
            get { return currentCityDataHeadID; }
        }

        /// <summary>
        /// der Kopfdatensatz der aktuell laufenden Simulation
        /// </summary>
        public static CityDataHead CurrentCityDataHead;

        /// <summary>
        /// Wenn auf der Settings unterseite Settings geändert werden werden diese auch hier im Simulationservice geändert ohne einen Webserver neustart
        /// </summary>
        /// <param name="setting"></param>
        public static void UpdateSimulationSettings(Setting setting) {
            simService.SetSettings(setting);

            settings = setting;
        }

        /// <summary>
        /// Wird aufgerufen wenn eine Simulation über das Dashboard von einem angemeldeten User gestartet wird
        /// </summary>
        /// <param name="simScenario"></param>
        /// <param name="duration"></param>
        public static void StartSimulation(SimScenario simScenario, TimeSpan duration) {
            simService.Run(simScenario, duration);

            simService.SimulationEnded += HandleStopSimulationEvent;

            CityDataHead head = new CityDataHead { 
                StartTime = (DateTime)simService.StartDateTimeReal,
                EndTime = (DateTime)simService.EndDateTimeReal,
                SimulationID = simScenario.SimScenarioID,
                State = "Running"
            };

            db.CityDataHeads.Add(head);
            db.SaveChanges();

            head.CityDataHeadID = (from cdh in db.CityDataHeads
                                   orderby cdh.CityDataHeadID descending
                                   select cdh.CityDataHeadID).FirstOrDefault();

            currentCityDataHeadID = head.CityDataHeadID;
            CurrentCityDataHead = head;

            SendCityDataHead();
        }

        /// <summary>
        /// Event wenn eine Simulation zuende gelaufen ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private static void HandleStopSimulationEvent(object sender, System.EventArgs a) {
            simService.SimulationEnded -= HandleStopSimulationEvent;

            StopSimulation();
        }

        /// <summary>
        /// Senden der aktuellen Kopfdaten an die Brwoser via dem Websocketserver
        /// </summary>
        public static void SendCityDataHead() {
            if (CurrentCityDataHead != null) {
                server.SendData(JsonConvert.SerializeObject(CurrentCityDataHead));
            }
        }

        /// <summary>
        /// Wenn eine Simulation manuell frühzeitig gestoppt wird
        /// </summary>
        public static void StopSimulation() {
            simService.Stop();

            CityDataHead head = db.CityDataHeads.Find(CurrentCityDataHeadID);

            head.EndTime = DateTime.Now;
            head.State = "Stopped";

            db.SaveChanges();

            CurrentCityDataHead = null;
            currentCityDataHeadID = null;

            server.SendData(JsonConvert.SerializeObject(head));
        }

        /// <summary>
        /// Setzen der Idle Values
        /// </summary>
        /// <param name="energyConsumption"></param>
        /// <param name="energyProductionSun"></param>
        /// <param name="energyProductionWind"></param>
        public static void SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind) {
            simService.SetIdleValues(energyConsumption, energyProductionSun, energyProductionWind);
        }

        /// <summary>
        /// Starten des Websocketclients und Verbindungsaufbau mit dem Raspberry
        /// </summary>
        public static void StartWebsocketClient() {
            client.RegisterEvents();
            client.Connect();
        }

        /// <summary>
        /// Falls der Connectionstring geändert wird kann hier der Websocketclient neugestartet werden
        /// </summary>
        public static void RestartWebsocketClient() {
            client.ReConnect();
        }

        /// <summary>
        /// Start des Websocketservers damit sich Browser damit verbinden können
        /// </summary>
        public static void StartWebsocketServer() {
            server.OpenConnection();
        }
    }
}