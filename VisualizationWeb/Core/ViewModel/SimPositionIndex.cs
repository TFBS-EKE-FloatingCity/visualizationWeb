using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.ViewModel
{
   public class SimPositionIndex
   {
      [Key]
      public int SimPositionID { get; set; }

      [Display(Name = "Sun")]
      public int SunValue { get; set; }

      [Display(Name = "Wind")]
      public int WindValue { get; set; }

      [Display(Name = "Consumption")]
      public int EnergyConsumptionValue { get; set; }

      [Display(Name = "Time")]
      [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
      public DateTime TimeRegistered { get; set; }
   }
}