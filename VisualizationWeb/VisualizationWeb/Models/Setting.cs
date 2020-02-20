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
        public double WindMax { get; set; }
        public double SunMax { get; set; }
        public double ConsumptionMax { get; set; }
        public bool WindActive { get; set; }
        public bool SunActive { get; set; }
        public bool ConsumptionActive { get; set; }

    }
}