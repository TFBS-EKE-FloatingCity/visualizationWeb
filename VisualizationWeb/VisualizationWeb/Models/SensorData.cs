using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VisualizationWeb.Models
{
    public class SensorData
    {
        [Key]
        public int SensorDataID { get; set; }

        public int SensorID { get; set; }

        public DateTime MeasureTime { get; set; }

        public double SValue { get; set; }

        [JsonIgnore]
        //[IgnoreDataMember]
        public virtual Sensor Sensor { get; set; }
    }

    public class SensorDataApi
    {
        public int Sensor { get; set; }

        public int Value0 { get; set; }

        public int Value1 { get; set; }

        public int Timestamp { get; set; } 
    }
   
}