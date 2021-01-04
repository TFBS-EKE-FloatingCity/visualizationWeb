﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Simulation.Library.ViewModels.SimPositionVM
{
    public class SimPositionCreateAndEditViewModel
    {
        [Key]
        public int SimPositionID { get; set; }

        public int SunValue { get; set; }

        public int WindValue { get; set; }

        public int EnergyBalanceValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateRegistered { get; set; }

        public int SimScenarioID { get; set; }
    }
}