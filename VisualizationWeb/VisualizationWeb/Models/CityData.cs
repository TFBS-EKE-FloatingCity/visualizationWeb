using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models
{
    public class CityData
    {
        [Key]
        public string UUID { get; set; }
        public int?  CityDataHeadID { get; set; }
        public short USonicInner1 { get; set; }
        public short USonicOuter1 { get; set; }
        public short Pump1 { get; set; }


        public short USonicInner2 { get; set; }
        public short USonicOuter2 { get; set; }
        public short Pump2 { get; set; }


        public short USonicInner3 { get; set; }
        public short USonicOuter3 { get; set; }
        public short Pump3 { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime MesurementTime { get; set; }

        public int? SimulationID { get; set; }

        public int? WindMax { get; set; }
        public short? WindCurrent { get; set; }
        public int? SunMax { get; set; }
        public short? SunCurrent { get; set; }
        public int? ConsumptionMax { get; set; }
        public short? ConsumptionCurrent { get; set; }
        public bool SimulationActive { get; set; }
        public DateTime? Simulationtime { get; set; }
        public decimal? TimeFactor { get; set; }

    }
}