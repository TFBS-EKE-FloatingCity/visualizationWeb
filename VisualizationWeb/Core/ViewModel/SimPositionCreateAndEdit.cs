using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.ViewModel
{
   public class SimPositionCreateAndEdit
   {
      [Key]
      public int SimPositionID { get; set; }

      [Range(0, 100, ErrorMessage = "Only Values between 0 and 100 are allowed!")]
      [Display(Name = "Sun Value (in %)")]
      public int SunValue { get; set; }

      [Range(0, 100, ErrorMessage = "Only Values between 0 and 100 are allowed!")]
      [Display(Name = "Wind Value (in %)")]
      public int WindValue { get; set; }

      [Range(0, 100, ErrorMessage = "Only Values between 0 and 100 are allowed!")]
      [Display(Name = "Consumption Value (in %)")]
      public int EnergyConsumptionValue { get; set; }

      [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
      [Display(Name = "Time Registered")]
      [DataType(DataType.Time, ErrorMessage = "Only Time Value is allowed!")]
      public DateTime TimeRegistered { get; set; }

      public int SimScenarioID { get; set; }
   }
}