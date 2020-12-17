using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Models.ViewModel.SimScenarioVM
{
    public class SimScenarioIndexViewModel
    {
        [Key]
        public int SimScenarioID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        public decimal TimeFactor { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}