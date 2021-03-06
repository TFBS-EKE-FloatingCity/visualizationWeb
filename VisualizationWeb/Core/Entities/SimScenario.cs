using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Entities
{
   public class SimScenario
   {
      [Key]
      public int SimScenarioID { get; set; }

      [Required]
      [StringLength(100, MinimumLength = 1)]
      public string Title { get; set; }

      /// <summary>
      ///   StartDate = First Position OrderedBy TimeRegistered
      /// </summary>
      [NotMapped]
      public DateTime? StartDate
      {
         get
         {
            return SimPositions?.OrderBy(x => x.TimeRegistered.TimeOfDay).FirstOrDefault()?.TimeRegistered;
         }
      }

      /// <summary>
      ///   EndDate = Last Position OrderedBy TimeRegistered
      /// </summary>
      [NotMapped]
      public DateTime? EndDate
      {
         get
         {
            return SimPositions?.OrderByDescending(x => x.TimeRegistered.TimeOfDay).FirstOrDefault()?.TimeRegistered;
         }
      }

      [StringLength(500)]
      public string Notes { get; set; }

      public List<SimPosition> SimPositions { get; set; }

      public bool IsSimulationRunning { get; set; }

      public TimeSpan GetDuration()
      {
         if (SimPositions != null && SimPositions.Count >= 2)
         {
            List<SimPosition> positions = SimPositions.OrderBy(p => p.TimeRegistered).ToList();
            return positions.Last().TimeRegistered - positions.First().TimeRegistered;
         }
         return new TimeSpan(0);
      }
   }
}