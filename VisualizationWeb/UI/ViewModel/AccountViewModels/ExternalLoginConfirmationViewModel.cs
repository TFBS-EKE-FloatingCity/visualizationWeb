using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel.AccountViewModels
{
   public class ExternalLoginConfirmationViewModel
   {
      [Required]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }
}