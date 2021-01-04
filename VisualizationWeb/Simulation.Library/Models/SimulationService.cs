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
        public SimScenario Simulation { get; set; }

        public event EventHandler SimulationStarted;
        public event EventHandler SimulationEnded;
        #endregion

        #region Constructor
        public SimulationService(SimScenario simulation, TimeSpan duration)
        {
            Simulation = simulation;
            Duration = duration;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Runs the simulation.
        /// </summary>
        public void Run()
        {
            if(Simulation.SimPositions != null && Simulation.SimPositions.Count >= 2)  //Only runs the Simulation if the Simulation is valid. The Simulation is valid when there are at least two Positions.
            {
                StartDateTimeReal = DateTime.Now;
                _prevPosition = Simulation.SimPositions.OrderBy(p => p.DateRegistered).First();
                _nextPosition = Simulation.SimPositions.OrderBy(p => p.DateRegistered).Skip(1).First();
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
            decimal simTimeTicksValue = CalculationHelper.Lerp(Simulation.StartDate.Value.Ticks, Simulation.EndDate.Value.Ticks, factor);   //Translates the percentage value to the actual time value in the simulated time span
            return new DateTime((long)simTimeTicksValue);
        }

        public int GetWindValue(DateTime timeStamp)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Returns the definied maximum energyproduction of the windturbines.
        /// </summary>
        public int GetMaxEnergyProductionWind()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the definied maximum energyproduction of the suncollectors.
        /// </summary>
        public int GetMaxEnergyProductionSun()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the definied maximum energyconsumption of the city.
        /// </summary>
        public int GetMaxEnergyConsumption()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns if the current is running.
        /// </summary>
        public bool IsSimulationRunning()
        {
            return _isRunning;
        }

        protected virtual bool isTimeStampValid(DateTime timeStamp)
        {
            DateTime? simTimeStamp = GetSimulatedTimeStamp(timeStamp);
            return simTimeStamp >= Simulation.StartDate && simTimeStamp <= Simulation.EndDate;
        }

        protected void refreshPositions(DateTime simTimeStamp)
        {
            if(simTimeStamp >= _nextPosition.DateRegistered && simTimeStamp <= _prevPosition.DateRegistered)
            {
                return;
            }

            _prevPosition = Simulation.SimPositions.OrderBy(p => p.DateRegistered).Where(p => p.DateRegistered <= simTimeStamp).LastOrDefault();
            _nextPosition = Simulation.SimPositions.OrderBy(p => p.DateRegistered).Where(p => p.DateRegistered >= simTimeStamp).FirstOrDefault();
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

        #endregion
    }
}
