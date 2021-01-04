using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Models
{
    public class SimScenario
    {
        [Key]
        public int SimScenarioID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        public decimal TimeFactor { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime? StartDate
        {
            get;
            set;
        }

        public DateTime? EndDate 
        {
            get;
            set;
        }

        public ICollection<SimPosition> SimPositions { get; set; }

    }
}
