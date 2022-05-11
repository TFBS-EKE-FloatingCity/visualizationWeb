using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.Models
{
   public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
