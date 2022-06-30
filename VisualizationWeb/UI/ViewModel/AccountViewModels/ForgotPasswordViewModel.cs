using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel.AccountViewModels
{
   public class ForgotPasswordViewModel
   {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }
}