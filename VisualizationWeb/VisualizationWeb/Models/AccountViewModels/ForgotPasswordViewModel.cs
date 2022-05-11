using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.Models
{
   public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
