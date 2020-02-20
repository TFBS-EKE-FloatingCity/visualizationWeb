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
        [Display(Name = "Simulationszeit")]
        public DateTime SimTime { get; set; }
        [Range(0.0, 100.0)]
        public double Wind { get; set; }
        [Range(0.0, 100.0)]
        public double Sun { get; set; }
        [Range(0.0, 100.0)]
        public double Consumption { get; set; }

        public virtual SimType SimType { get; set; }
    }
}