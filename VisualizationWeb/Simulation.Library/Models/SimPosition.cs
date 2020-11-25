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

        [Required]
        public int SunValue { get; set; }

        [Required]
        public int WindValue { get; set; }

        [Required]
        public int EnergyBalanceValue { get; set; }

        [Required]
        public DateTime DateRegistered { get; set; }

        public int SimScenarioID { get; set; }

        public SimScenario SimScenario { get; set; }
    }
}
