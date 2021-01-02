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

        public void Stop()
        {
            _isRunning = false;
            StartDateTimeReal = null;
            onSimulationEnded();
        }

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

        public int GetMaxEnergyProductionWind()
        {
            throw new NotImplementedException();
        }

        public int GetMaxEnergyProductionSun()
        {
            throw new NotImplementedException();
        }

        public int GetMaxEnergyConsumption()
        {
            throw new NotImplementedException();
        }

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

        protected virtual void onSimulationStarted()
        {
            SimulationStarted?.Invoke(this, new EventArgs());
        }

        protected virtual void onSimulationEnded()
        {
            SimulationEnded?.Invoke(this, new EventArgs());
        }

        public DateTime? GetSimulationStartedTimeStamp()
        {
            return _startDateTimeReal;
        }

        #endregion
    }
}
