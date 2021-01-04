using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Helpers.Temporary {
    public class SimulationService : ISimulationService {
        public void Dispose() {
            throw new NotImplementedException();
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

        public int GetMaxEnergyConsumption() {
            return 0;
        }

        public int GetMaxEnergyProductionSun() {
            return 0;
        }

        public int GetMaxEnergyProductionWind() {
            return 0;
        }

        public DateTime? GetSimulatedTimeStamp(DateTime timeStamp) {
            return null;
        }

        public bool IsSimulationRunning() {
            return false;
        }

        public void Run() {
            throw new NotImplementedException();
        }

        public void Stop() {
            throw new NotImplementedException();
        }
    }
}