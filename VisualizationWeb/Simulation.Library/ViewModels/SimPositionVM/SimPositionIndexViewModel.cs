using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Simulation.Library.ViewModels.SimPositionVM
{
    public class SimPositionIndexViewModel
    {
        [Key]
        public int SimPositionID { get; set; }

        public int SunValue { get; set; }

        public int WindValue { get; set; }

        public int EnergyConsumptionValue { get; set; }

        public DateTime TimeRegistered { get; set; }
    }
}