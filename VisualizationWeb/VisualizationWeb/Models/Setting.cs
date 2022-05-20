using Simulation.Library.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.Models
{
   public class Setting : ISimulationServiceSettings
   {
      [Key]
      public int SettingID { get; set; }

      [Display(Name = "Maximum Wind(MW)")]
      [Range(0, Int32.MaxValue)]
      public int WindMax { get; set; }

      [Display(Name = "Maximum Sun(MW)")]
      [Range(0, Int32.MaxValue)]
      public int SunMax { get; set; }

      [Display(Name = "Maximum Consumption(MW)")]
      [Range(0, Int32.MaxValue)]
      public int ConsumptionMax { get; set; }

      [StringLength(500)]
      [Display(Name = "Connection String to Raspberry")]
      public string rbPiConnectionString { get; set; }

      [StringLength(500)]
      [Display(Name = "Connection String from Browser to Server")]
      public string browserConnectionString { get; set; }
   }
}