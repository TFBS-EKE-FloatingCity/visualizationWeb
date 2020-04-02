using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models.ViewModel
{
    public class SimulationTimesVM
    {
        public double SimFactor { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}