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

        public int SunValue { get; set; }

        public int WindValue { get; set; }

        public int EnergyConsumptionValue { get; set; }

        public DateTime TimeRegistered { get; set; }

        public int SimScenarioID { get; set; }

        public SimScenario SimScenario { get; set; }
    }
}
