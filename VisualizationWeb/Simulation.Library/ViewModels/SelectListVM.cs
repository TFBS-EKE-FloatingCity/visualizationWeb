using System.ComponentModel.DataAnnotations;

namespace Simulation.Library.ViewModels
{
   public class vmSelectListItem
    {
        [Key]
        public int ValueMember { get; set; }
        public string DisplayMember { get; set; }
    }
}