using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class Sensor
    {
        [Key]
        public int SensorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Sensoren")]
        public string Title { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        public decimal Factor { get; set; }
        [StringLength(10)]
        public string Einheiten { get; set; }
        [StringLength(3)]
        public string SCode { get; set; }
        [StringLength(50)]
        public string Prefix { get; set; }

        public virtual ICollection<SensorData> SensorDatas { get; set; }
    }
}