﻿using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel.AccountViewModels
{
   public class LoginViewModel
   {
      [Required]
      [Display(Name = "Email")]
      [EmailAddress]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }
   }
}