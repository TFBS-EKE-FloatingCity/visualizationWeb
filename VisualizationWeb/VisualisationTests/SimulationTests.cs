using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualisationTests
{
    [TestClass]
    public class SimulationTests
    {
        #region SimulationService Tests
        private SimScenario getTestScenario()
        {
            SimPosition pos1 = new SimPosition { SimPositionID = 1, SunValue = 20, WindValue = 5, EnergyBalanceValue = 40, DateRegistered = new DateTime(2021, 01, 01, 8, 0, 0) };
            SimPosition pos2 = new SimPosition { SimPositionID = 1, SunValue = 100, WindValue = 10, EnergyBalanceValue = 20, DateRegistered = new DateTime(2021, 01, 01, 10, 0, 0) };
            SimPosition pos3 = new SimPosition { SimPositionID = 1, SunValue = 40, WindValue = 20, EnergyBalanceValue = 50, DateRegistered = new DateTime(2021, 01, 01, 20, 0, 0) };

            return new SimScenario { SimScenarioID = 1, Title = "TestScenario", SimPositions = new List<SimPosition> { pos1, pos2, pos3 } };
        }
        
        [TestMethod]
        public void SimulationServiceRun_ValidScenario()
        {
            SimScenario scenario = getTestScenario();
            ISimulationService service = new SimulationService(scenario, new TimeSpan(1, 0, 0));

            ISimulationService eventSender = null;
            service.SimulationStarted += delegate (object sender, EventArgs e) 
            { 
                eventSender = (SimulationService)sender; 
            };

            service.Run();
            Assert.AreEqual(service, eventSender);
            Assert.IsTrue(service.IsSimulationRunning());
        }

        [TestMethod]
        public void SimulationServiceRun_InvalidScenario()
        {
            SimScenario scenario = new SimScenario();
            ISimulationService service = new SimulationService(scenario, new TimeSpan(1, 0, 0));

            ISimulationService eventSender = null;
            service.SimulationStarted += delegate (object sender, EventArgs e) 
            { 
                eventSender = (SimulationService)sender; 
            };

            service.Run();
            Assert.IsTrue(eventSender is null);
            Assert.IsTrue(service.IsSimulationRunning() is false);
        }

        [TestMethod]
        public void SimulationServiceStop()
        {
            SimScenario scenario = getTestScenario();
            ISimulationService service = new SimulationService(scenario, new TimeSpan(1, 0, 0));

            ISimulationService eventSender = null;
            service.SimulationEnded += delegate (object sender, EventArgs e)
            {
                eventSender = (SimulationService)sender;
            };

            service.Run();
            service.Stop();
            Assert.AreEqual(service, eventSender);
            Assert.IsTrue(service.IsSimulationRunning() is false);
        }

        [TestMethod]
        [DataRow(5, 9)]
        [DataRow(10, 10)]
        [DataRow(15, 11)]
        [DataRow(30, 14)]
        [DataRow(45, 17)]
        public void SimulationService_SimulatedTimeStamp(int minutesPassed, int expectedSimulationHour)
        {
            SimScenario scenario = getTestScenario();
            ISimulationService service = new SimulationService(scenario, new TimeSpan(1, 0, 0));

            service.Run();
            DateTime testRealTime = service.GetSimulationStartedTimeStamp().Value + new TimeSpan(0, minutesPassed, 0);
            
            Assert.AreEqual(new DateTime(2021, 01, 01, expectedSimulationHour, 0, 0), service.GetSimulatedTimeStamp(testRealTime));
        }
        #endregion
    }
}
