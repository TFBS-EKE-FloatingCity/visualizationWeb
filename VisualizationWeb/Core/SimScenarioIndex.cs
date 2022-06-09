using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccess.Entities
{
   public class SimScenarioIndex
   {
      [Key]
      public int SimScenarioID { get; set; }

      [Required]
      [StringLength(100, MinimumLength = 1)]
      public string Title { get; set; }

      [Display(Name = "Start Time")]
      public DateTime? StartDate
      {
         get
         {
            return SimPositions?.OrderBy(x => x.TimeRegistered.TimeOfDay).FirstOrDefault()?.TimeRegistered;
         }
      }

      [Display(Name = "End Time")]
      public DateTime? EndDate
      {
         get
         {
            return SimPositions?.OrderByDescending(x => x.TimeRegistered.TimeOfDay).FirstOrDefault()?.TimeRegistered;
         }
      }

      public bool isChecked { get; set; }

      public IEnumerable<SimPositionIndex> SimPositions { get; set; }
   }
}