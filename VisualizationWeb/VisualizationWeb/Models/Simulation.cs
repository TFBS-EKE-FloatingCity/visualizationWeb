using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class Simulation
    {
        [Key]
        public int SimulationID { get; set; }
        public int SimTypeID { get; set; }
        [Display(Name ="Echte Start Zeit")]
        public DateTime RealStartTime { get; set; }
        [Display(Name = "Calculation factor")]
        public double SimFactor { get; set; }
        [Display(Name = "Start der Simulation")]
        public DateTime StartTime { get; set; }

        public virtual ICollection<SensorData> SensorDatas { get; set; }
    }
}