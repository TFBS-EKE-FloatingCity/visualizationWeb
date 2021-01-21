using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Models
{
    public class SimulationServiceSetting
    {        
        [Key]
        public string OptionName { get; set; }
              
        public int OptionValue { get; set; }
        public string Description { get; set; }
    }
}
