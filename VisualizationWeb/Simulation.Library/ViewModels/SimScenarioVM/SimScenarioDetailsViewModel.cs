using Simulation.Library.ViewModels.SimPositionVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Simulation.Library.Models.ViewModels.SimScenarioVM
{
    public class SimScenarioDetailsViewModel
    {
        [Key]
        public int SimScenarioID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        public decimal TimeFactor { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public IEnumerable<SimPositionBindingViewModel> SimPositions { get; set; }
    }
}