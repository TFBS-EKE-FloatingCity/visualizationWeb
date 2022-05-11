using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models {
    public class CityDataHead {
        public int CityDataHeadID { get; set; }

        //Running / Stopped
        public string State { get; set; }

        public int SimulationID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [JsonIgnore]
        public ICollection<CityData> CityDatas { get; set; }
    }
}