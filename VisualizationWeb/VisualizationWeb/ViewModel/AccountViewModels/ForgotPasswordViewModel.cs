using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.ViewModel.AccountViewModels
{
   public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
