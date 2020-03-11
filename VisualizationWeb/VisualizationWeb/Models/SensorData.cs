using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class SensorData
    {
        [Key]
        public int SensorDataID { get; set; }
        public int SimulationTimeID { get; set; }
        public int MyProperty { get; set; }
        [Display(Name = "Echtzeit")]
        public DateTime RealTime { get; set; }
        [Display(Name = "Simulationszeit")]
        public DateTime SimulationTime { get; set; }
        public int SensorID { get; set; }
        public double SValue { get; set; }

        public virtual Sensor Sensor { get; set; }
        public virtual Simulation Simulation { get; set; }
    }
    public class SensorDataApi
    {
        public int Sensor { get; set; }
        public int Value0 { get; set; }
        public int Value1 { get; set; }
    }
   
}