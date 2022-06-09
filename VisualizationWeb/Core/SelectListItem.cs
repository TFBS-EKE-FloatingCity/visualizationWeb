using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
   public class SelectListItem
   {
      [Key]
      public int ValueMember { get; set; }

      public string DisplayMember { get; set; }
   }
}