using Newtonsoft.Json;
using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisualizationWeb.Models;

namespace VisualizationWeb.Helpers {
    public static class SingletonHolder {

        private static ApplicationDbContext db = new ApplicationDbContext();

        private static readonly SimulationService simService = new SimulationService();
        private static readonly WebsocketServer server = new WebsocketServer();
        private static readonly WebSocketClient client = new WebSocketClient(server, simService);

        private static int? currentCityDataHeadID;

        public static int? CurrentCityDataHeadID {
            get { return currentCityDataHeadID; }
        }

        public static CityDataHead CurrentCityDataHead;

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

        private static void HandleStopSimulationEvent(object sender, System.EventArgs a) {
            simService.SimulationEnded -= HandleStopSimulationEvent;

            StopSimulation();
        }

        public static void SendCityDataHead() {
            if (CurrentCityDataHead != null) {
                server.SendData(JsonConvert.SerializeObject(CurrentCityDataHead));
            }
        }

        public static void StopSimulation() {
            simService.Stop();

            CityDataHead head = db.CityDataHeads.Find(CurrentCityDataHeadID);

            head.EndTime = DateTime.Now;
            head.State = "Stopped";

            db.SaveChanges();

            CurrentCityDataHead = null;

            server.SendData(JsonConvert.SerializeObject(head));
        }

        public static void SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind) {
            simService.SetIdleValues(energyConsumption, energyProductionSun, energyProductionWind);
        }

        public static void StartWebsocketClient() {
            client.RegisterEvents();
            client.Connect();
        }

        public static void StartWebsocketServer() {
            server.OpenConnection();
        }
    }
}