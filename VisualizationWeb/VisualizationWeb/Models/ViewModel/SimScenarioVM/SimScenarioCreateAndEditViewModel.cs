using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models.ViewModel.SimScenarioVM
{
    public class SimScenarioCreateAndEditViewModel
    {
        [Key]
        public int SimScenarioID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        public decimal TimeFactor { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}