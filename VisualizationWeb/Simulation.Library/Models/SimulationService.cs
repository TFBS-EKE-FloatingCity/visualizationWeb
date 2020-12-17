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
            StartDateTimeReal = DateTime.Now;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
            StartDateTimeReal = null;
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
            throw new NotImplementedException();
        }

        public int? GetEnergyProductionWind(DateTime timeStamp)
        {
            throw new NotImplementedException();
        }

        public int? GetEnergyProductionSun(DateTime timeStamp)
        {
            throw new NotImplementedException();
        }

        public int? GetEnergyConsumption(DateTime timeStamp)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        #endregion
    }
}
