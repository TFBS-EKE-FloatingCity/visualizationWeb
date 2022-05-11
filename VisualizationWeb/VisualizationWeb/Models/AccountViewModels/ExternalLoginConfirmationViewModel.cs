using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.Models
{
   public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
