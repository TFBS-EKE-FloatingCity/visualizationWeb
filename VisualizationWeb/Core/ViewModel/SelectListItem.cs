using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.ViewModel
{
   public class SelectListItem
   {
      [Key]
      public int ValueMember { get; set; }

      public string DisplayMember { get; set; }
   }
}