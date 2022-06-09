using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
   public class SimPositionBindingViewModel
   {
      [Key]
      public int SimPositionID { get; set; }

      public int SunValue { get; set; }

      public int WindValue { get; set; }

      public int EnergyConsumptionValue { get; set; }

      public DateTime TimeRegistered { get; set; }
   }
}