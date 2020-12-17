using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Simulation.Library.Models.ViewModels.SimPositionVM
{
    public class SimPositionCreateAndEditViewModel
    {
        [Key]
        public int SimPositionID { get; set; }

        public int SunValue { get; set; }

        public int WindValue { get; set; }

        public int EnergyBalanceValue { get; set; }

        public DateTime DateRegistered { get; set; }

        public int SimScenarioID { get; set; }
    }
}