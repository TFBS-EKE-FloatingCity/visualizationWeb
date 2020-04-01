using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class SimData
    {
        [Key]
        public int SimDataID { get; set; }

        public int SimTypeID { get; set; }

        [Display(Name = "Simulationtime")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime SimTime { get; set; }

        [Range(0.0, 100.0)]
        [Display(Name = "Maximum Wind")]
        public double Wind { get; set; }

        [Range(0.0, 100.0)]
        [Display(Name = "Maximum Sun")]
        public double Sun { get; set; }

        [Range(0.0, 100.0)]
        [Display(Name = "Consumption")]
        public double Consumption { get; set; }

        [JsonIgnore]
        public virtual SimType SimType { get; set; }
       
    }
}