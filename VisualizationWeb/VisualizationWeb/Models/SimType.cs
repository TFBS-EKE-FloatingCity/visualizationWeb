using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class SimType
    {
        [Key]
        public int SimTypeID { get; set; }
        [StringLength(50)]
        [Display (Name = "Simulation")]
        public string Title { get; set; }
        [Display(Name = "Calculation factor")]
        public double SimFactor { get; set; }
        [StringLength(200)]
        [Display(Name = "Description")]
        public string Notes { get; set; }
        [Display(Name = "Starttime")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }
        [Display(Name = "Interval")]
        public TimeSpan Interval { get; set; }
        [Display(Name = "Endtime")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }


        public virtual ICollection<SimData> SimDatas { get; set; }

    }
}