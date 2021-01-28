using Simulation.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class Setting : ISimulationServiceSettings
    {
        [Key]
        public int SettingID { get; set; }
        [Display(Name = "Maximum Wind")]
        [Range(0, Int32.MaxValue)]
        public int WindMax { get; set; }
        [Display(Name = "Maximum Sun")]
        [Range(0, Int32.MaxValue)]
        public int SunMax { get; set; }
        [Display(Name = "Maximum Consumption")]
        [Range(0, Int32.MaxValue)]
        public int ConsumptionMax { get; set; }

        [StringLength(500)]
        [Display(Name = "Connection String to Raspberry")]
        public string rbPiConnectionString { get; set; }
    }
}