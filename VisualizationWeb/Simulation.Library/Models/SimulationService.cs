using Newtonsoft.Json.Linq;
using Simulation.Library.Calculations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Models
{
    public class SimulationService : IDisposable, ISimulationService
    {
        #region Properties
        private TimeSpan _duration;
        private DateTime? _startDateTimeReal;
        private DateTime? _endDateTimeReal;
        private bool _isRunning;
        private SimPosition _prevPosition;
        private SimPosition _nextPosition;
        private int _maxEnergyProductionWind;
        private int _maxEnergyProductionSun;
        private int _maxEnergyConsumption;
        private SimScenario _simScenario;
        private decimal _timeFactor;

        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
        }
        public DateTime? StartDateTimeReal
        {
            get
            {
                return _startDateTimeReal;
            }
            private set
            {
                _startDateTimeReal = value;
                if (value != null && Duration != null)
                {
                    EndDateTimeReal = value.Value + Duration;
                }
                else
                {
                    EndDateTimeReal = null;
                }
            }
        }
        public DateTime? EndDateTimeReal
        {
            get { return _endDateTimeReal; }
            private set { _endDateTimeReal = value; }
        }
        public int MaxEnergyProductionWind
        {
            get => _maxEnergyProductionWind;
        }
        public int MaxEnergyProductionSun
        {
            get => _maxEnergyProductionSun;
        }
        public int MaxEnergyConsumption
        {
            get => _maxEnergyConsumption;
        }
        public bool IsSimulationRunning
        {
            get => _isRunning;
        }
        public decimal TimeFactor
        {
            get => _timeFactor;
        }
        public int SimulationScenarioId
        {
            get => _simScenario == null ? -1 : _simScenario.SimScenarioID;
        }

        public event EventHandler SimulationStarted;
        public event EventHandler SimulationEnded;
        #endregion

        #region Constructor
        public SimulationService()
        {
            readConfig(@"SimulationServiceConfig.json");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reads the maximum values from the SimulationServiceConfig.json and writes it to the fields.
        /// </summary>
        private void readConfig(string configPath)
        {
            using (StreamReader reader = new StreamReader(configPath))
            {
                JObject jdata = JObject.Parse(reader.ReadToEnd());
                _maxEnergyConsumption = jdata["SimulationData"]["Consumption"]["Maximum"].ToObject<int>();
                _maxEnergyProductionSun = jdata["SimulationData"]["Sun"]["Maximum"].ToObject<int>();
                _maxEnergyProductionWind = jdata["SimulationData"]["Wind"]["Maximum"].ToObject<int>();
            }
        }

        /// <summary>
        /// Runs the simulation.
        /// </summary>
        public void Run(SimScenario scenario, TimeSpan duration)
        {
            if (setupForRunning(scenario, duration)) 
            {
                StartDateTimeReal = DateTime.Now;
                _prevPosition = _simScenario.SimPositions.OrderBy(p => p.DateRegistered).First();
                _nextPosition = _simScenario.SimPositions.OrderBy(p => p.DateRegistered).Skip(1).First();
                _isRunning = true;
                onSimulationStarted();
            }
            else
            {
                throw new Exception($"Couldn't run the Simulation. Please check if the parameters were valid. " +
                    $"The parameters are valid when the scenario contains at least two positions and the duration is longer than 0");
            }
        }

        /// <summary>
        /// Stops the simulation.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            StartDateTimeReal = null;
            _timeFactor = 0;
            _simScenario = null;
            onSimulationEnded();
        }

        /// <summary>
        /// Stops the Simulation on Dispose.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Updates to the given timeStamp and returns the simulatedTimeStamp. Returns Null if the requested timeStamp is outside the simulated time.
        /// </summary>
        private DateTime? update(DateTime timeStamp)
        {
            if (_isRunning == false)
            {
                return null;
            }
            if (isTimeStampValid(timeStamp) == false)
            {
                if(timeStamp > EndDateTimeReal)     //This means we reached the end of the simulation. So the Service is getting stopped.
                {
                    Stop();
                }
                return null;
            }

            DateTime simTimeStamp = GetSimulatedTimeStamp(timeStamp).Value;
            refreshPositions(simTimeStamp);

            if (_prevPosition is null || _nextPosition is null)                             //If either of the positions is null means that the given timeStamp is outside the SimulationTimeRange.
            {                                                                               //This shouldn't occur since we check if the given timeStamp is valid.
                throw new Exception("At least one SimulationPosition was null.");
            }

            return simTimeStamp;
        }

        /// <summary>
        /// Calculates the current Time in the Simulation for the given timeStamp.
        /// </summary>
        /// <returns>Returns the current time in the simulation. Null if the simulation is not running.</returns>
        public DateTime? GetSimulatedTimeStamp(DateTime timeStamp)
        {
            if (StartDateTimeReal == null)
            {
                return null;
            }
            decimal factor = CalculationHelper.InverseLerp(0, Duration.Ticks, timeStamp.Ticks - StartDateTimeReal.Value.Ticks);          //Calculates the percentage of how far the simulation is in the real time
            decimal simTimeTicksValue = CalculationHelper.Lerp(_simScenario.StartDate.Value.Ticks, _simScenario.EndDate.Value.Ticks, factor);   //Translates the percentage value to the actual time value in the simulated time span
            return new DateTime((long)simTimeTicksValue);
        }


        /// <summary>
        /// Calculates the percental simulated energyproduction for the given timeStamp for the wind turbines.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        public int? GetEnergyProductionWind(DateTime timeStamp)
        {
            DateTime? simTimeStamp = update(timeStamp);
            if (simTimeStamp == null)
            {
                return null;
            }

            return CalculationHelper.GetValue(_prevPosition.DateRegistered.Ticks,
                _prevPosition.WindValue,
                _nextPosition.DateRegistered.Ticks,
                _nextPosition.WindValue,
                simTimeStamp.Value.Ticks);
        }

        /// <summary>
        /// Calculates the percental simulated energyproduction for the given timeStamp for the suncollectors.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        public int? GetEnergyProductionSun(DateTime timeStamp)
        {
            DateTime? simTimeStamp = update(timeStamp);
            if (simTimeStamp == null)
            {
                return null;
            }

            return CalculationHelper.GetValue(_prevPosition.DateRegistered.Ticks,
                _prevPosition.SunValue,
                _nextPosition.DateRegistered.Ticks,
                _nextPosition.SunValue,
                simTimeStamp.Value.Ticks);
        }

        /// <summary>
        /// Calculates the percental simulated energyconsumption of the city for the given timeStamp.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        public int? GetEnergyConsumption(DateTime timeStamp)
        {
            DateTime? simTimeStamp = update(timeStamp);
            if (simTimeStamp == null)
            {
                return null;
            }

            return CalculationHelper.GetValue(_prevPosition.DateRegistered.Ticks,
                _prevPosition.EnergyBalanceValue,
                _nextPosition.DateRegistered.Ticks,
                _nextPosition.EnergyBalanceValue,
                simTimeStamp.Value.Ticks);
        }

        /// <summary>
        /// Checks if a given TimeStamp is valid.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns>True if the simulation is running and the simulated time is between the first and the last position of the Scenario.</returns>
        protected virtual bool isTimeStampValid(DateTime timeStamp)
        {
            if(_isRunning == false)
            {
                return false;
            }
            DateTime? simTimeStamp = GetSimulatedTimeStamp(timeStamp);
            return simTimeStamp >= _simScenario.StartDate && simTimeStamp <= _simScenario.EndDate;
        }

        /// <summary>
        /// Recalculates the fields _nextPosition and _prevPosition.
        /// </summary>
        protected void refreshPositions(DateTime simTimeStamp)
        {
            if (simTimeStamp >= _nextPosition.DateRegistered && simTimeStamp <= _prevPosition.DateRegistered)
            {
                return;
            }

            _prevPosition = _simScenario.SimPositions.OrderBy(p => p.DateRegistered).Where(p => p.DateRegistered <= simTimeStamp).LastOrDefault();
            _nextPosition = _simScenario.SimPositions.OrderBy(p => p.DateRegistered).Where(p => p.DateRegistered >= simTimeStamp).FirstOrDefault();
        }

        /// <summary>
        /// Invokes the Event "SimulationStarted".
        /// </summary>
        protected virtual void onSimulationStarted()
        {
            SimulationStarted?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Invokes the Event "SimulationEnded".
        /// </summary>
        protected virtual void onSimulationEnded()
        {
            SimulationEnded?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Returns the actual StartedDateTime. Null if the Simulation is not running.
        /// </summary>
        public DateTime? GetSimulationStartedTimeStamp()
        {
            return _startDateTimeReal;
        }

        /// <summary>
        /// Calculates the energy balance for the given timeStamp. Negativ if the Consumption is higher than the production of Sun and Wind.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns>The simulated energy balance. Null if the simulation is not running</returns>
        public int? GetEnergyBalance(DateTime timeStamp)
        {
            if (_isRunning == false || isTimeStampValid(timeStamp) == false)
            {
                return null;
            }

            int? windValue = GetEnergyProductionWind(timeStamp);
            int? sunValue = GetEnergyProductionSun(timeStamp);
            int? consumptionValue = GetEnergyConsumption(timeStamp);

            int? balanceValue = windValue + sunValue - consumptionValue;
            if (balanceValue >= 0)
            {
                return (int)CalculationHelper.InverseLerp(0, MaxEnergyProductionWind + MaxEnergyProductionSun, balanceValue.Value);
            }
            else
            {
                return (int)CalculationHelper.InverseLerp(0, MaxEnergyConsumption, balanceValue.Value);
            }
        }

        /// <summary>
        /// Validates the Scenario and Duration and sets the Properties which are required for the simulation to run.
        /// </summary>
        /// <returns>True if setup successfull</returns>
        private bool setupForRunning(SimScenario scenario, TimeSpan duration)
        {
            if(!isValidScenario(scenario) || !isValidDuration(duration))
            {
                return false;
            }

            if (_isRunning)
            {
                Stop();
            }

            _simScenario = scenario;
            _duration = duration;
            _timeFactor = CalculationHelper.InverseLerp(0, Duration.Ticks, _simScenario.GetDuration().Ticks);
            return true;
        }

        /// <summary>
        /// Validates the Scenario.
        /// </summary>
        private bool isValidScenario(SimScenario scenario)
        {
            return scenario != null && scenario.SimPositions != null && scenario.SimPositions.Count >= 2;
        }

        /// <summary>
        /// Validates the duration.
        /// </summary>
        private bool isValidDuration(TimeSpan duration)
        {
            return duration.Ticks > 0;
        }
        #endregion
    }
}
