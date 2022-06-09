using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
   public class SimScenarioCreateAndEditViewModel
   {
      [Key]
      public int SimScenarioID { get; set; }

      [Required]
      [StringLength(100, MinimumLength = 1)]
      public string Title { get; set; }

      [StringLength(500)]
      public string Notes { get; set; }
   }
}