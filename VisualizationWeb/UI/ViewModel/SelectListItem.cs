using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel
{
   public class SelectListItem
   {
      [Key]
      public int ValueMember { get; set; }

      public string DisplayMember { get; set; }
   }
}