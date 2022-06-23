using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.ViewModel
{
   public class SimPositionBinding
   {
      [Key]
      public int SimPositionID { get; set; }

      public int SunValue { get; set; }

      public int WindValue { get; set; }

      public int EnergyConsumptionValue { get; set; }

      public DateTime TimeRegistered { get; set; }
   }
}