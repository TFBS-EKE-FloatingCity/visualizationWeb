using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models.ViewModel {
    public class JsonResponseVM {
        public string status { get; set; }
        public string uuid { get; set; }
        public int? sun { get; set; }
        public int? wind { get; set; }
        public int? energyBalance { get; set; }
    }
}