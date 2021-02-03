using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Simulation.Library.Calculations;
using Simulation.Library.Models;
using Simulation.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisualizationWeb.Models;

namespace VisualisationTests
{
    [TestClass]
    public class SimulationTests
    {
        #region CalculationHelper Tests
        [TestMethod]
        [DataRow(0, 10, 0.5, 5)]
        [DataRow(-10, 0, 0.5, -5)]
        [DataRow(-10, 10, 0.5, 0)]
        [DataRow(0, 20, 0.75, 15)]
        public void LerpTest(double min, double max, double value, double expected)
        {
            decimal dMin = Convert.ToDecimal(min);
            decimal dMax = Convert.ToDecimal(max);
            decimal dValue = Convert.ToDecimal(value);
            decimal dExpected = Convert.ToDecimal(expected);

            decimal result = InterpolationHelper.Lerp(dMin, dMax, dValue);
            Assert.AreEqual(dExpected, result);
        }

        [TestMethod]
        [DataRow(0, 10, 5, 0.5)]
        [DataRow(0, -10, 5, -0.5)]
        [DataRow(0, -10, -5, 0.5)]
        public void InverseLerpTest(double min, double max, double value, double expected)
        {
            decimal dMin = Convert.ToDecimal(min);
            decimal dMax = Convert.ToDecimal(max);
            decimal dValue = Convert.ToDecimal(value);
            decimal dExpected = Convert.ToDecimal(expected);

            decimal result = InterpolationHelper.InverseLerp(dMin, dMax, dValue);
            Assert.AreEqual(dExpected, result);
        }

        [TestMethod]
        public void GetValueTest()
        {
            int result1 = (int)InterpolationHelper.GetValue(0, 0, 10, 100, 5);
            int result2 = (int)InterpolationHelper.GetValue(-10, 0, 10, 100, -5);
            int result3 = (int)InterpolationHelper.GetValue(-10, -100, 10, 100, -5);
            int result4 = (int)InterpolationHelper.GetValue(10, 100, 0, 0, 5);
            int result5 = (int)InterpolationHelper.GetValue(0, 0, 0, 0, 0);

            Assert.AreEqual(50, result1);
            Assert.AreEqual(25, result2);
            Assert.AreEqual(-50, result3);
            Assert.AreEqual(50, result4);
            Assert.AreEqual(0, result5);

            Assert.ThrowsException<DivideByZeroException>(() => InterpolationHelper.GetValue(0, 0, 0, 10, 0));
        }
        #endregion

        #region SimulationService Tests
        private SimScenario getValidTestScenario()
        {
            // Avoid Making Changes for this Method! Most tests in this TestClass rely on this Method.

            SimPosition pos1 = new SimPosition { SimPositionID = 1, SunValue = 20, WindValue = 5, EnergyConsumptionValue = 40, TimeRegistered = new DateTime(2021, 01, 01, 8, 0, 0) };
            SimPosition pos2 = new SimPosition { SimPositionID = 1, SunValue = 100, WindValue = 10, EnergyConsumptionValue = 20, TimeRegistered = new DateTime(2021, 01, 01, 10, 0, 0) };
            SimPosition pos3 = new SimPosition { SimPositionID = 1, SunValue = 40, WindValue = 20, EnergyConsumptionValue = 50, TimeRegistered = new DateTime(2021, 01, 01, 20, 0, 0) };

            return new SimScenario { SimScenarioID = 1, Title = "TestScenario", SimPositions = new List<SimPosition> { pos1, pos2, pos3 } };
        }

        private Setting getConfig()
        {
            return new Setting
            {
                ConsumptionMax = 10000,
                SunMax = 10000,
                WindMax = 10000,
            };
        }

        private void simulationService_AssertRunning(ISimulationService service)
        {
            Assert.AreNotEqual(-1, service.SimulationScenarioId);
            Assert.AreNotEqual(null, service.StartDateTimeReal);
            Assert.AreNotEqual(null, service.EndDateTimeReal);
            Assert.AreNotEqual(false, service.IsSimulationRunning);
            Assert.AreNotEqual(0m, service.TimeFactor);
            Assert.AreNotEqual(null, service.GetEnergyBalance(DateTime.Now));
            Assert.AreNotEqual(null, service.GetEnergyConsumption(DateTime.Now));
            Assert.AreNotEqual(null, service.GetEnergyProductionSun(DateTime.Now));
            Assert.AreNotEqual(null, service.GetEnergyProductionWind(DateTime.Now));
            Assert.AreNotEqual(null, service.GetSimulatedTimeStamp(DateTime.Now));
        }

        private void simulationService_AssertNotRunning(ISimulationService service)
        {
            Assert.AreEqual(-1, service.SimulationScenarioId);
            Assert.AreEqual(null, service.StartDateTimeReal);
            Assert.AreEqual(null, service.EndDateTimeReal);
            Assert.AreEqual(false, service.IsSimulationRunning);
            Assert.AreEqual(1m, service.TimeFactor);
            Assert.AreEqual(0, service.GetEnergyBalance(DateTime.Now));
            Assert.AreEqual(0, service.GetEnergyConsumption(DateTime.Now));
            Assert.AreEqual(0, service.GetEnergyProductionSun(DateTime.Now));
            Assert.AreEqual(0, service.GetEnergyProductionWind(DateTime.Now));
            Assert.AreEqual(null, service.GetSimulatedTimeStamp(DateTime.Now));
        }

        [TestMethod]
        public void SimulationService_TestConstructor()
        {
            JObject jdata;
            using (StreamReader reader = new StreamReader(@"SimulationServiceConfig.json"))
            {
                jdata = JObject.Parse(reader.ReadToEnd());
            }
            ISimulationServiceSettings settings = getConfig();
            ISimulationService service = new SimulationService(settings);
            Assert.AreEqual(settings.ConsumptionMax, service.MaxEnergyConsumption);
            Assert.AreEqual(settings.SunMax, service.MaxEnergyProductionSun);
            Assert.AreEqual(settings.WindMax, service.MaxEnergyProductionWind);
            simulationService_AssertNotRunning(service);
        }

        [TestMethod]
        public void SimulationServiceRun_ValidParameters()
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, Int32.MaxValue);
            ISimulationService service = new SimulationService(getConfig());

            ISimulationService eventSender = null;
            service.SimulationStarted += delegate (object sender, EventArgs e)
            {
                eventSender = (SimulationService)sender;
            };

            service.Run(scenario, duration);
            Assert.AreEqual(service, eventSender);
            simulationService_AssertRunning(service);
        }

        [TestMethod]
        public void SimulationServiceRun_InvalidParameters()
        {
            ISimulationService service = new SimulationService(getConfig());

            ISimulationService eventSender = null;
            service.SimulationStarted += delegate (object sender, EventArgs e)
            {
                eventSender = (SimulationService)sender;
            };

            Assert.ThrowsException<Exception>(() => service.Run(null, new TimeSpan(1,0,0)));
            Assert.ThrowsException<Exception>(() => service.Run(getValidTestScenario(), new TimeSpan()));
            Assert.IsTrue(eventSender is null);
            simulationService_AssertNotRunning(service);
        }

        [TestMethod]
        public void SimulationServiceStop()
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            ISimulationService eventSender = null;
            service.SimulationEnded += delegate (object sender, EventArgs e)
            {
                eventSender = (SimulationService)sender;
            };

            service.Run(scenario, duration);
            service.Stop();
            Assert.AreEqual(service, eventSender);
            simulationService_AssertNotRunning(service);
        }

        [TestMethod]
        public void SimulationService_StopAfterDuration()
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(0, 0, 1);
            ISimulationService service = new SimulationService(getConfig());

            ISimulationService eventSender = null;
            service.SimulationEnded += delegate (object sender, EventArgs e)
            {
                eventSender = (SimulationService)sender;
            };

            service.Run(scenario, duration);
            Thread.Sleep(2000);
            Assert.AreEqual(service, eventSender);
            simulationService_AssertNotRunning(service);
        }

        [TestMethod]
        public void SimulationService_StopOnRunningSecondSimulation()
        {
            SimScenario scenario1 = getValidTestScenario();
            SimScenario scenario2 = getValidTestScenario();
            scenario2.SimScenarioID = 2;
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            ISimulationService eventSender = null;
            service.SimulationEnded += delegate (object sender, EventArgs e)
            {
                eventSender = (SimulationService)sender;
            };

            service.Run(scenario1, duration);
            service.Run(scenario2, duration);
            Assert.AreEqual(service, eventSender);
            Assert.AreEqual(scenario2.SimScenarioID, service.SimulationScenarioId);
        }

        [TestMethod]
        public void SimulationService_EndDateTimeReal()
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            service.Run(scenario, duration);
            Assert.AreEqual(service.StartDateTimeReal.Value + duration, service.EndDateTimeReal);
        }

        [TestMethod]
        [DataRow(1, 12)]
        [DataRow(2, 6)]
        [DataRow(12, 1)]
        [DataRow(24, 0.5)]
        public void SimulationService_TimeFactor(int durationHours, double expectedFactor)
        {
            decimal expected = Convert.ToDecimal(expectedFactor);
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(durationHours, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            service.Run(scenario, duration);
            Assert.AreEqual(service.TimeFactor, expected);
        }

        [TestMethod]
        [DataRow(-1, 0)]
        [DataRow(0, 40)]
        [DataRow(5, 30)]
        [DataRow(10, 20)]
        [DataRow(35, 35)]
        [DataRow(60, 50)]
        [DataRow(61, 0)]
        public void SimulationService_GetEnergyConmption(int minutesPassed, int? expectedValue)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(expectedValue, service.GetEnergyConsumption(testRealTime));
        }

        [TestMethod]
        [DataRow(-1, 0)]
        [DataRow(0, 5)]
        [DataRow(6, 8)]
        [DataRow(10, 10)]
        [DataRow(60, 20)]
        [DataRow(61, 0)]
        public void SimulationService_GetEnergyProductionWind(int minutesPassed, int? expectedValue)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(expectedValue, service.GetEnergyProductionWind(testRealTime));
        }

        [TestMethod]
        [DataRow(-1, 0)]
        [DataRow(0, 20)]
        [DataRow(10, 100)]
        [DataRow(35, 70)]
        [DataRow(60, 40)]
        [DataRow(61, 0)]
        public void SimulationService_GetEnergyProductionSun(int minutesPassed, int? expectedValue)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(expectedValue, service.GetEnergyProductionSun(testRealTime));
        }

        [TestMethod]
        [DataRow(-1, 0)]
        [DataRow(0, -15)]
        [DataRow(21, 36)]
        [DataRow(60, 5)]
        [DataRow(61, 0)]
        public void SimulationService_GetEnergyBalance(int minutesPassed, int expectedValue)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(expectedValue, service.GetEnergyBalance(testRealTime));
        }

        [TestMethod]
        [DataRow(5, 9)]
        [DataRow(10, 10)]
        [DataRow(15, 11)]
        [DataRow(30, 14)]
        [DataRow(45, 17)]
        public void SimulationService_GetSimulatedTimeStamp(int minutesPassed, int expectedSimulationHour)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService(getConfig());

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(new DateTime(2021, 01, 01, expectedSimulationHour, 0, 0), service.GetSimulatedTimeStamp(testRealTime));
        }

        [TestMethod]
        [DataRow(0,0,0,0,0,0)]
        [DataRow(30,40,50,30,40,50)]
        [DataRow(100,100,100,100,100,100)]
        [DataRow(-1,-1,-1,0,0,0)]
        [DataRow(101,101,101,0,0,0)]
        public void SimulationService_SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind, int expectedEnergyConsumption, int expectedEnergyProductionSun, int expectedEnergyProductionWind)
        {
            ISimulationService service = new SimulationService(getConfig());

            service.SetIdleValues(energyConsumption, energyProductionSun, energyProductionWind);

            Assert.AreEqual(expectedEnergyConsumption, service.GetEnergyConsumption(DateTime.Now));
            Assert.AreEqual(expectedEnergyProductionSun, service.GetEnergyProductionSun(DateTime.Now));
            Assert.AreEqual(expectedEnergyProductionWind, service.GetEnergyProductionWind(DateTime.Now));
        }

        #endregion
    }
}
