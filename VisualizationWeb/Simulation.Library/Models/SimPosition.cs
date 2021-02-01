using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Models
{
    public class SimPosition
    {
        [Key]
        public int SimPositionID { get; set; }

        /// <summary>
        /// Sun Percent Value 0 - 100
        /// </summary>
        public int SunValue { get; set; }

        /// <summary>
        /// Wind Percent Value 0 - 100
        /// </summary>
        public int WindValue { get; set; }

        /// <summary>
        /// Engergy Consumption Percent Value 0 - 100
        /// </summary>
        public int EnergyConsumptionValue { get; set; }

        /// <summary>
        /// Time Values were measured (only Time)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime TimeRegistered { get; set; }

        public int SimScenarioID { get; set; }

        public SimScenario SimScenario { get; set; }
    }
}
