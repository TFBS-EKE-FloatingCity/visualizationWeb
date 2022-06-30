using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel.AccountViewModels
{
   public class ForgotViewModel
   {
      [Required]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }
}