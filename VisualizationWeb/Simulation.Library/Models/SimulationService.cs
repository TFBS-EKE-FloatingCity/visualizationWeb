using Simulation.Library.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Models
{
    public class SimulationService
    {
        #region Properties

        private TimeSpan _duration;
        private DateTime _startDateTimeReal;
        private DateTime _endDateTimeReal;

        public TimeSpan Duration
        {
            get 
            { 
                return _duration; 
            }
            set 
            { 
                _duration = value;
                if(value != null && StartDateTimeReal != null)
                {
                    EndDateTimeReal = StartDateTimeReal + value;
                }
            }
        }
        public DateTime StartDateTimeReal
        {
            get 
            { 
                return _startDateTimeReal; 
            }
            set 
            { 
                _startDateTimeReal = value;
                if(value != null && Duration != null)
                {
                    EndDateTimeReal = value + Duration;
                }
            }
        }
        public DateTime EndDateTimeReal
        {
            get { return _endDateTimeReal; }
            set { _endDateTimeReal = value; }
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
        /// <summary>
        /// Calculates the current Time in the Simulation.
        /// </summary>
        private DateTime getSimulatedTimeStamp()
        {
            decimal factor = CalculationHelper.InverseLerp(StartDateTimeReal.Ticks, EndDateTimeReal.Ticks, DateTime.Now.Ticks);      //Calculates the percentage of how far the simulation is in the real time
            decimal simTimeTicksValue = CalculationHelper.Lerp(Simulation.StartDate.Value.Ticks, Simulation.EndDate.Value.Ticks, factor);   //Translates the percentage value to the actual time value in the simulated time span
            return new DateTime((long)simTimeTicksValue);
        }

        #endregion
    }
}
