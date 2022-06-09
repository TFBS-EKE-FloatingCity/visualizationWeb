using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel.ManageViewModels
{
   public class AddPhoneNumberViewModel
   {
      [Required]
      [Phone]
      [Display(Name = "Phone Number")]
      public string Number { get; set; }
   }
}