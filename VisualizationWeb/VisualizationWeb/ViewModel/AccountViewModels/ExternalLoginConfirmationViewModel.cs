using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.ViewModel.AccountViewModels
{
   public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
