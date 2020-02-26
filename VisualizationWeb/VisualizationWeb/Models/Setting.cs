using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class Setting
    {
        [Key]
        public int SettingID { get; set; }
        [Display(Name = "Maximum Wind ")]
        public double WindMax { get; set; }
        [Display(Name = "Maximum Sun ")]
        public double SunMax { get; set; }
        [Display(Name = "Maximum Consumption")]
        public double ConsumptionMax { get; set; }
        [Display(Name = "Wind Active")]
        public bool WindActive { get; set; }
        [Display(Name = "Sun Active")]
        public bool SunActive { get; set; }
        [Display(Name = "Consumption Active")]
        public bool ConsumptionActive { get; set; }
    }
}