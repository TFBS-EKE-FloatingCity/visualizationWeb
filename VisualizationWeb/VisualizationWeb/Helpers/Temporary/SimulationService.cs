using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Helpers.Temporary {
    public class SimulationService : ISimulationService {
        public int SimulationScenarioId { get; set; }

        public int MaxEnergyProductionWind { get; set; }

        public int MaxEnergyProductionSun { get; set; }

        public int MaxEnergyConsumption { get; set; }

        public bool IsSimulationRunning { get; set; }

        public DateTime? StartDateTimeReal { get; set; }

        public decimal TimeFactor { get; set; }

        public event EventHandler SimulationStarted;
        public event EventHandler SimulationEnded;

        public void Dispose() {
            throw new NotImplementedException();
        }

        public int? GetEnergyBalance(DateTime timeStamp) {
            return null;
        }

        public int? GetEnergyConsumption(DateTime timeStamp) {
            return null;
        }

        public int? GetEnergyProductionSun(DateTime timeStamp) {
            return null;
        }

        public int? GetEnergyProductionWind(DateTime timeStamp) {
            return null;
        }

        public DateTime? GetSimulatedTimeStamp(DateTime timeStamp) {
            return null;
        }

        public void Run() {
            throw new NotImplementedException();
        }

        //public void SetSimulationScenario(SimScenario scenario) {
        //    throw new NotImplementedException();
        //}

        public void Stop() {
            throw new NotImplementedException();
        }
    }
}