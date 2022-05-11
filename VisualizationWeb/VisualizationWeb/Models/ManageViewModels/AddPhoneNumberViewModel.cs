using System.ComponentModel.DataAnnotations;

namespace VisualizationWeb.Models
{
   public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}