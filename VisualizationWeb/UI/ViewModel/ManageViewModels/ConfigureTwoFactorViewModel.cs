using System.Collections.Generic;

namespace UI.ViewModel.ManageViewModels
{
   public class ConfigureTwoFactorViewModel
   {
      public string SelectedProvider { get; set; }
      public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
   }
}