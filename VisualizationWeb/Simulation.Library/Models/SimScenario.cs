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

        public int TimeFactor { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime? StartDate
        {
            get
            {
                if (SimPositions?.Count == 0)
                {
                    return null;
                }
                return SimPositions.First().DateRegistered;
            }
            set{ StartDate = value; }
        }

        public DateTime? EndDate 
        {
            get
            {
                if (SimPositions?.Count == 0)
                {
                    return null;
                }
                return SimPositions.Last().DateRegistered;
            }
            set { EndDate = value; }
        }

        public ICollection<SimPosition> SimPositions { get; set; }
    }
}
