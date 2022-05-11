using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models.ViewModel {
    public class JsonDataVM {
        public string uuid { get; set; }
        public payloadVM payload { get; set; }
    }

    public class payloadVM {
        //Muss string sein da der UNIX Timestamp nicht direkt beim deserialisieren in ein Datetime geparst werden kann
        public string timestamp { get; set; }

        public List<modulesVM> modules { get; set; }
    }

    public class modulesVM {
        public string sector { get; set; }
        public int sensorOutside { get; set; }
        public int sensorInside { get; set; }
        public int pumpLevel { get; set; }
    }
}