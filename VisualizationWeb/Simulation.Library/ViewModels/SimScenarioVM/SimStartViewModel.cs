using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.ViewModels.SimScenarioVM
{
    public class SimStartViewModel
    {
        public SimStartViewModel()
        {
            Scenarios = new List<SimScenarioIndexViewModel>();
        }
        public TimeSpan Duration { get; set; }

        public IEnumerable<SimScenarioIndexViewModel> Scenarios { get; set; }
    }
}
