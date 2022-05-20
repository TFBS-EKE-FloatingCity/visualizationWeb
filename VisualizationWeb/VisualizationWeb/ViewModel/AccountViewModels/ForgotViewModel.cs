using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.ViewModel.AccountViewModels
{
   public class ForgotViewModel
   {
      [Required]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }
}