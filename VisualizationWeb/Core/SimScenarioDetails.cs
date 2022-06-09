using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccess.Entities
{
   public class SimScenarioDetails
   {
      [Key]
      public int SimScenarioID { get; set; }

      [Required]
      [StringLength(100, MinimumLength = 1)]
      public string Title { get; set; }

      [Display(Name = "Start Time")]
      [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
      public DateTime? StartDate
      {
         get
         {
            return SimPositions?.OrderBy(x => x.TimeRegistered.TimeOfDay).FirstOrDefault()?.TimeRegistered;
         }
      }

      [Display(Name = "End Time")]
      [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
      public DateTime? EndDate
      {
         get
         {
            return SimPositions?.OrderByDescending(x => x.TimeRegistered.TimeOfDay).FirstOrDefault()?.TimeRegistered;
         }
      }

      [StringLength(500)]
      public string Notes { get; set; }

      public IEnumerable<SimPositionIndex> SimPositions { get; set; }

      public SimScenarioDetails()
      {
         SimPositions = new List<SimPositionIndex>();
      }
   }
}