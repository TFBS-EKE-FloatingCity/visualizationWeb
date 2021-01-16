using Newtonsoft.Json.Linq;
using Simulation.Library.Calculations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Simulation.Library.Models
{
    public class SimulationService : IDisposable, ISimulationService
    {
        #region Properties
        private TimeSpan _duration;
        private DateTime? _startDateTimeReal;
        private DateTime? _endDateTimeReal;
        private bool _isScenarioRunning;
        private SimPosition _prevPosition;
        private SimPosition _nextPosition;
        private int _maxEnergyProductionWind;
        private int _maxEnergyProductionSun;
        private int _maxEnergyConsumption;
        private SimScenario _simScenario;
        private decimal _timeFactor;
        private int _idleEnergyProductionWind;
        private int _idleEnergyProductionSun;
        private int _idleEnergyConsumption;
        private Timer _timer;

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
            get => _isScenarioRunning;
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
            SetIdleValues(0, 0, 0);
            readConfig();
            //readConfig(@"SimulationServiceConfig.json");
            _timer = new Timer();
            _timeFactor = 1;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reads the maximum values from the SimulationServiceConfig.json and writes it to the fields.
        /// </summary>
        private void readConfig(string configPath = "")
        {

            JObject jdata = JObject.Parse(SimulationServiceConfig.Config);
            _maxEnergyConsumption = jdata["SimulationData"]["Consumption"]["Maximum"].ToObject<int>();
            _maxEnergyProductionSun = jdata["SimulationData"]["Sun"]["Maximum"].ToObject<int>();
            _maxEnergyProductionWind = jdata["SimulationData"]["Wind"]["Maximum"].ToObject<int>();

            //using (StreamReader reader = new StreamReader(configPath)) {
            //    JObject jdata = JObject.Parse(reader.ReadToEnd());
            //    _maxEnergyConsumption = jdata["SimulationData"]["Consumption"]["Maximum"].ToObject<int>();
            //    _maxEnergyProductionSun = jdata["SimulationData"]["Sun"]["Maximum"].ToObject<int>();
            //    _maxEnergyProductionWind = jdata["SimulationData"]["Wind"]["Maximum"].ToObject<int>();
            //}
        }

        /// <summary>
        /// Runs the simulation.
        /// </summary>
        public void Run(SimScenario scenario, TimeSpan duration)
        {
            if (setupForRunning(scenario, duration)) 
            {
                StartDateTimeReal = DateTime.Now;
                _timer.Start();
                _prevPosition = _simScenario.SimPositions.OrderBy(p => p.TimeRegistered).First();
                _nextPosition = _simScenario.SimPositions.OrderBy(p => p.TimeRegistered).Skip(1).First();
                _isScenarioRunning = true;
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
            _isScenarioRunning = false;
            StartDateTimeReal = null;
            _timeFactor = 1;
            _simScenario = null;
            _prevPosition = null;
            _nextPosition = null;
            _timer.Stop();
            _timer.Elapsed -= onSimDurationElapsed;
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
            if (_isScenarioRunning == false || isTimeStampValid(timeStamp) == false)
            {
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
        /// <returns>The percental simulated energyproduction. Returns idle values if no Simulation is running.</returns>
        public int GetEnergyProductionWind(DateTime timeStamp)
        {
            DateTime? simTimeStamp = update(timeStamp);
            if (simTimeStamp == null)
            {
                return _idleEnergyProductionWind;
            }

            return CalculationHelper.GetValue(_prevPosition.TimeRegistered.Ticks,
                _prevPosition.WindValue,
                _nextPosition.TimeRegistered.Ticks,
                _nextPosition.WindValue,
                simTimeStamp.Value.Ticks);
        }

        /// <summary>
        /// Calculates the percental simulated energyproduction for the given timeStamp for the suncollectors.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Returns idle values if no Simulation is running.</returns>
        public int GetEnergyProductionSun(DateTime timeStamp)
        {
            DateTime? simTimeStamp = update(timeStamp);
            if (simTimeStamp == null)
            {
                return _idleEnergyProductionSun;
            }

            return CalculationHelper.GetValue(_prevPosition.TimeRegistered.Ticks,
                _prevPosition.SunValue,
                _nextPosition.TimeRegistered.Ticks,
                _nextPosition.SunValue,
                simTimeStamp.Value.Ticks);
        }

        /// <summary>
        /// Calculates the percental simulated energyconsumption of the city for the given timeStamp.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Returns idle values if no Simulation is running.</returns>
        public int GetEnergyConsumption(DateTime timeStamp)
        {
            DateTime? simTimeStamp = update(timeStamp);
            if (simTimeStamp == null)
            {
                return _idleEnergyConsumption;
            }

            return CalculationHelper.GetValue(_prevPosition.TimeRegistered.Ticks,
                _prevPosition.EnergyConsumptionValue,
                _nextPosition.TimeRegistered.Ticks,
                _nextPosition.EnergyConsumptionValue,
                simTimeStamp.Value.Ticks);
        }

        /// <summary>
        /// Checks if a given TimeStamp is valid.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns>True if the simulation is running and the simulated time is between the first and the last position of the Scenario.</returns>
        protected virtual bool isTimeStampValid(DateTime timeStamp)
        {
            if(_isScenarioRunning == false)
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
            if (simTimeStamp >= _nextPosition.TimeRegistered && simTimeStamp <= _prevPosition.TimeRegistered)
            {
                return;
            }

            _prevPosition = _simScenario.SimPositions.OrderBy(p => p.TimeRegistered).Where(p => p.TimeRegistered <= simTimeStamp).LastOrDefault();
            _nextPosition = _simScenario.SimPositions.OrderBy(p => p.TimeRegistered).Where(p => p.TimeRegistered >= simTimeStamp).FirstOrDefault();
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
        /// <returns>The simulated energy balance. Returns idle values if no Simulation is running.</returns>
        public int GetEnergyBalance(DateTime timeStamp)
        {
            int windValue = GetEnergyProductionWind(timeStamp);
            int sunValue = GetEnergyProductionSun(timeStamp);
            int consumptionValue = GetEnergyConsumption(timeStamp);

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

            if (_isScenarioRunning)
            {
                Stop();     // Stops the previous Scenario if there is one running.
            }

            _simScenario = scenario;
            _duration = duration;
            _timeFactor = CalculationHelper.InverseLerp(0, Duration.Ticks, _simScenario.GetDuration().Ticks);
            _timer.Interval = duration.TotalMilliseconds;
            _timer.Elapsed += onSimDurationElapsed;
            return true;
        }

        /// <summary>
        /// Validates the Scenario.
        /// </summary>
        protected bool isValidScenario(SimScenario scenario)
        {
            return scenario != null && scenario.SimPositions != null && scenario.SimPositions.Count >= 2;
        }

        /// <summary>
        /// Validates the duration.
        /// </summary>
        protected bool isValidDuration(TimeSpan duration)
        {
            return duration.Ticks > 0;
        }

        /// <summary>
        /// Sets the Idle Values which are sent while no Scenario is running.
        /// </summary>
        /// <param name="energyConsumption">The energy consumption of the city in percent. Must be a value between 0 and 100 percent.</param>
        /// <param name="energyProductionSun">The energy production of the suncollectors in percent. Must be a value between 0 and 100 percent.</param>
        /// <param name="energyProductionWind">The energy production of the wind turbines in percent. Must be a value between 0 and 100 percent.</param>
        public void SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind)
        {
            _idleEnergyConsumption = energyConsumption >= 0 && energyConsumption <= 100 ? energyConsumption : _idleEnergyConsumption;
            _idleEnergyProductionSun = energyProductionSun >= 0 && energyProductionSun <= 100 ? energyProductionSun : _idleEnergyProductionSun;
            _idleEnergyProductionWind = energyProductionWind >= 0 && energyProductionWind <= 100 ? energyProductionWind : _idleEnergyProductionWind;
        }

        /// <summary>
        /// Method which stops the running SimScenario after a set TimeInterval.
        /// </summary>
        protected void onSimDurationElapsed(object sender, ElapsedEventArgs e)
        {
            Stop();
        }
        #endregion
    }
}
