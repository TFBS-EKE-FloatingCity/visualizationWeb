using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class SimulationHistory
    {
        [Key]
        public int HistoryID { get; set; }

        [Required]
        public DateTime RealStartTime { get; set; }

        [Required]
        public int SimTypeID { get; set; }

        public DateTime? Canceled { get; set; }
        //public int UserID { get; set; }

        public virtual SimType SimType { get; set; }
    }
}