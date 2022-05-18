using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.ViewModel
{
   public class SelectListItemVM
   {
      [Key]
      public int ValueMember { get; set; }
      public string DisplayMember { get; set; }
   }
}
