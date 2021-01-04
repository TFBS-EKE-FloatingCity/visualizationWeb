using Simulation.Library.Calculations;
using System;
using System.Collections.Generic;
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
        private DateTime _endDateTimeReal;
        private bool _isRunning;
        private SimPosition _prevPosition;
        private SimPosition _nextPosition;
        private int _maxEnergyProductionWind;
        private int _maxEnergyProductionSun;
        private int _maxEnergyConsumption;
        private SimScenario _simulation;
        private decimal _timeFactor;

        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                if (value != null && StartDateTimeReal != null)
                {
                    EndDateTimeReal = StartDateTimeReal.Value + value;
                }
            }
        }
        public DateTime? StartDateTimeReal
        {
            get
            {
                return _startDateTimeReal;
            }
            set
            {
                _startDateTimeReal = value;
                if (value != null && Duration != null)
                {
                    EndDateTimeReal = value.Value + Duration;
                }
            }
        }
        public DateTime EndDateTimeReal
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
            get => _simulation.SimScenarioID;
        }

        public event EventHandler SimulationStarted;
        public event EventHandler SimulationEnded;
        #endregion

        #region Constructor
        public SimulationService(SimScenario simulation, TimeSpan duration)
        {
            _simulation = simulation;
            Duration = duration;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Runs the simulation.
        /// </summary>
        public void Run()
        {
            if(_simulation.SimPositions != null && _simulation.SimPositions.Count >= 2)  //Only runs the Simulation if the Simulation is valid. The Simulation is valid when there are at least two Positions.
            {
                StartDateTimeReal = DateTime.Now;
                _prevPosition = _simulation.SimPositions.OrderBy(p => p.DateRegistered).First();
                _nextPosition = _simulation.SimPositions.OrderBy(p => p.DateRegistered).Skip(1).First();
                _isRunning = true;
                onSimulationStarted();
            }
        }

        /// <summary>
        /// Stops the simulation.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            StartDateTimeReal = null;
            onSimulationEnded();
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
            decimal factor = CalculationHelper.InverseLerp(StartDateTimeReal.Value.Ticks, EndDateTimeReal.Ticks, timeStamp.Ticks);          //Calculates the percentage of how far the simulation is in the real time
            decimal simTimeTicksValue = CalculationHelper.Lerp(_simulation.StartDate.Value.Ticks, _simulation.EndDate.Value.Ticks, factor);   //Translates the percentage value to the actual time value in the simulated time span
            return new DateTime((long)simTimeTicksValue);
        }

        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Calculates the percental simulated energyproduction for the given timeStamp for the wind turbines.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        public int? GetEnergyProductionWind(DateTime timeStamp)
        {
            if (_isRunning == false || isTimeStampValid(timeStamp) == false)
            {
                return null;
            }

            DateTime simTimeStamp = GetSimulatedTimeStamp(timeStamp).Value;
            refreshPositions(timeStamp);

            if (_prevPosition is null || _nextPosition is null)                             //If either of the positions is null means that the given timeStamp is outside the SimulationTimeRange.
            {                                                                               //This shouldn't occur since we check if the given timeStamp is valid.
                throw new Exception("At least one SimulationPosition was null.");
            }

            return CalculationHelper.GetValue(_prevPosition.DateRegistered.Ticks, 
                _prevPosition.WindValue, 
                _nextPosition.DateRegistered.Ticks, 
                _nextPosition.WindValue, 
                simTimeStamp.Ticks);
        }

        /// <summary>
        /// Calculates the percental simulated energyproduction for the given timeStamp for the suncollectors.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        public int? GetEnergyProductionSun(DateTime timeStamp)
        {
            if (_isRunning == false || isTimeStampValid(timeStamp) == false)
            {
                return null;
            }

            DateTime simTimeStamp = GetSimulatedTimeStamp(timeStamp).Value;
            refreshPositions(timeStamp);

            if (_prevPosition is null || _nextPosition is null)                             //If either of the positions is null means that the given timeStamp is outside the SimulationTimeRange.
            {                                                                               //This shouldn't occur since we check if the given timeStamp is valid.
                throw new Exception("At least one SimulationPosition was null.");
            }

            return CalculationHelper.GetValue(_prevPosition.DateRegistered.Ticks,
                _prevPosition.SunValue,
                _nextPosition.DateRegistered.Ticks,
                _nextPosition.SunValue,
                simTimeStamp.Ticks);
        }

        /// <summary>
        /// Calculates the percental simulated energyconsumption of the city for the given timeStamp.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        public int? GetEnergyConsumption(DateTime timeStamp)
        {
            if (_isRunning == false || isTimeStampValid(timeStamp) == false)
            {
                return null;
            }

            DateTime simTimeStamp = GetSimulatedTimeStamp(timeStamp).Value;
            refreshPositions(timeStamp);

            if (_prevPosition is null || _nextPosition is null)                             //If either of the positions is null means that the given timeStamp is outside the SimulationTimeRange.
            {                                                                               //This shouldn't occur since we check if the given timeStamp is valid.
                throw new Exception("At least one SimulationPosition was null.");
            }

            return CalculationHelper.GetValue(_prevPosition.DateRegistered.Ticks,
                _prevPosition.EnergyBalanceValue,
                _nextPosition.DateRegistered.Ticks,
                _nextPosition.EnergyBalanceValue,
                simTimeStamp.Ticks);
        }

        protected virtual bool isTimeStampValid(DateTime timeStamp)
        {
            DateTime? simTimeStamp = GetSimulatedTimeStamp(timeStamp);
            return simTimeStamp >= _simulation.StartDate && simTimeStamp <= _simulation.EndDate;
        }

        protected void refreshPositions(DateTime simTimeStamp)
        {
            if(simTimeStamp >= _nextPosition.DateRegistered && simTimeStamp <= _prevPosition.DateRegistered)
            {
                return;
            }

            _prevPosition = _simulation.SimPositions.OrderBy(p => p.DateRegistered).Where(p => p.DateRegistered <= simTimeStamp).LastOrDefault();
            _nextPosition = _simulation.SimPositions.OrderBy(p => p.DateRegistered).Where(p => p.DateRegistered >= simTimeStamp).FirstOrDefault();
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

        public int? GetEnergyBalance(DateTime timeStamp)
        {
            throw new NotImplementedException();
        }

        public void SetSimulationScenario(SimScenario scenario)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
