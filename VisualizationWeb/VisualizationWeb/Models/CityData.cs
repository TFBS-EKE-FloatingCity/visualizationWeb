using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models {
    public class CityData {
        [Key]
        public int CityDataID { get; set; }

        public int USonicInner1 { get; set; }
        public int USonicOuter1 { get; set; }
        public int Pump1 { get; set; }


        public int USonicInner2 { get; set; }
        public int USonicOuter2 { get; set; }
        public int Pump2 { get; set; }


        public int USonicInner3 { get; set; }
        public int USonicOuter3 { get; set; }
        public int Pump3 { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}