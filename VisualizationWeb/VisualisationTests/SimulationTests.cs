using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Simulation.Library.Calculations;
using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            decimal result = CalculationHelper.Lerp(dMin, dMax, dValue);
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

            decimal result = CalculationHelper.InverseLerp(dMin, dMax, dValue);
            Assert.AreEqual(dExpected, result);
        }

        [TestMethod]
        public void GetValueTest()
        {
            int result1 = CalculationHelper.GetValue(0, 0, 10, 100, 5);
            int result2 = CalculationHelper.GetValue(-10, 0, 10, 100, -5);
            int result3 = CalculationHelper.GetValue(-10, -100, 10, 100, -5);
            int result4 = CalculationHelper.GetValue(10, 100, 0, 0, 5);
            int result5 = CalculationHelper.GetValue(0, 0, 0, 0, 0);

            Assert.AreEqual(50, result1);
            Assert.AreEqual(25, result2);
            Assert.AreEqual(-50, result3);
            Assert.AreEqual(50, result4);
            Assert.AreEqual(0, result5);

            Assert.ThrowsException<DivideByZeroException>(() => CalculationHelper.GetValue(0, 0, 0, 10, 0));
        }
        #endregion

        #region SimulationService Tests
        private SimScenario getValidTestScenario()
        {
            // Avoid Making Changes for this Method! Most tests in this TestClass rely on this Method.

            SimPosition pos1 = new SimPosition { SimPositionID = 1, SunValue = 20, WindValue = 5, EnergyConsumptionValue = 40, TimeRegistered = new DateTime(2021, 01, 01, 8, 0, 0) };
            SimPosition pos2 = new SimPosition { SimPositionID = 1, SunValue = 100, WindValue = 10, EnergyConsumptionValue = 20, TimeRegistered = new DateTime(2021, 01, 01, 10, 0, 0) };
            SimPosition pos3 = new SimPosition { SimPositionID = 1, SunValue = 40, WindValue = 20, EnergyConsumptionValue = 50, TimeRegistered = new DateTime(2021, 01, 01, 20, 0, 0) };

            return new SimScenario { SimScenarioID = 1, Title = "TestScenario", SimPositions = new List<SimPosition> { pos1, pos2, pos3 }, StartDate = pos1.TimeRegistered, EndDate = pos3.TimeRegistered };
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
            Assert.AreEqual(0m, service.TimeFactor);
            Assert.AreEqual(null, service.GetEnergyBalance(DateTime.Now));
            Assert.AreEqual(null, service.GetEnergyConsumption(DateTime.Now));
            Assert.AreEqual(null, service.GetEnergyProductionSun(DateTime.Now));
            Assert.AreEqual(null, service.GetEnergyProductionWind(DateTime.Now));
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
            int consumption = jdata["SimulationData"]["Consumption"]["Maximum"].ToObject<int>();
            int productionSun = jdata["SimulationData"]["Sun"]["Maximum"].ToObject<int>();
            int productionWind = jdata["SimulationData"]["Wind"]["Maximum"].ToObject<int>();

            ISimulationService service = new SimulationService();
            Assert.AreEqual(consumption, service.MaxEnergyConsumption);
            Assert.AreEqual(productionSun, service.MaxEnergyProductionSun);
            Assert.AreEqual(productionWind, service.MaxEnergyProductionWind);
            simulationService_AssertNotRunning(service);
        }

        [TestMethod]
        public void SimulationServiceRun_ValidParameters()
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService();

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
            ISimulationService service = new SimulationService();

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
            ISimulationService service = new SimulationService();

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
        public void SimulationService_StopOnTimeStampAfterDuration()
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService();

            ISimulationService eventSender = null;
            service.SimulationEnded += delegate (object sender, EventArgs e)
            {
                eventSender = (SimulationService)sender;
            };

            service.Run(scenario, duration);
            service.GetEnergyConsumption(service.EndDateTimeReal.Value + new TimeSpan(0, 0, 1));
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
            ISimulationService service = new SimulationService();

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
            ISimulationService service = new SimulationService();

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
            ISimulationService service = new SimulationService();

            service.Run(scenario, duration);
            Assert.AreEqual(service.TimeFactor, expected);
        }

        [TestMethod]
        [DataRow(-1, null)]
        [DataRow(0, 40)]
        [DataRow(5, 30)]
        [DataRow(10, 20)]
        [DataRow(35, 35)]
        [DataRow(60, 50)]
        [DataRow(61, null)]
        public void SimulationService_GetEnergyConmption(int minutesPassed, int? expectedValue)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService();

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(expectedValue, service.GetEnergyConsumption(testRealTime));
        }

        [TestMethod]
        [DataRow(-1, null)]
        [DataRow(0, 5)]
        [DataRow(6, 8)]
        [DataRow(10, 10)]
        [DataRow(60, 20)]
        [DataRow(61, null)]
        public void SimulationService_GetEnergyProductionWind(int minutesPassed, int? expectedValue)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService();

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(expectedValue, service.GetEnergyProductionWind(testRealTime));
        }

        [TestMethod]
        [DataRow(-1, null)]
        [DataRow(0, 20)]
        [DataRow(10, 100)]
        [DataRow(35, 70)]
        [DataRow(60, 40)]
        [DataRow(61, null)]
        public void SimulationService_GetEnergyProductionSun(int minutesPassed, int? expectedValue)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService();

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(expectedValue, service.GetEnergyProductionSun(testRealTime));
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(21)]
        [DataRow(60)]
        [DataRow(61)]
        public void SimulationService_GetEnergyBalance(int minutesPassed)
        {
            SimScenario scenario = getValidTestScenario();
            TimeSpan duration = new TimeSpan(1, 0, 0);
            ISimulationService service = new SimulationService();

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            int? windValue = service.GetEnergyProductionWind(testRealTime);
            int? sunValue = service.GetEnergyProductionSun(testRealTime);
            int? consumptionValue = service.GetEnergyConsumption(testRealTime);

            int? balanceValue = windValue + sunValue - consumptionValue;
            int? result;
            if (balanceValue == null)
            {
                result = null;
            }
            else if (balanceValue >= 0)
            {
                result = (int)CalculationHelper.InverseLerp(0, service.MaxEnergyProductionWind + service.MaxEnergyProductionSun, balanceValue.Value);
            }
            else
            {
                result = (int)CalculationHelper.InverseLerp(0, service.MaxEnergyConsumption, balanceValue.Value);
            }

            Assert.AreEqual(result, service.GetEnergyBalance(testRealTime));
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
            ISimulationService service = new SimulationService();

            service.Run(scenario, duration);
            DateTime testRealTime = service.StartDateTimeReal.Value + new TimeSpan(0, minutesPassed, 0);

            Assert.AreEqual(new DateTime(2021, 01, 01, expectedSimulationHour, 0, 0), service.GetSimulatedTimeStamp(testRealTime));
        }

        #endregion
    }
}
