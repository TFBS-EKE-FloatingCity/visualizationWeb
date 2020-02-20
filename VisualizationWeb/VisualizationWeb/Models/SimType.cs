﻿using System;
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
        public double SimFactor { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }

        public virtual ICollection<SimData> SimDatas { get; set; }

    }
}