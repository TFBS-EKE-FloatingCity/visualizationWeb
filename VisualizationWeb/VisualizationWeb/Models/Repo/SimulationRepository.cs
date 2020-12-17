using Simulation.Library.Models.ViewModels.SimPositionVM;
using Simulation.Library.Models.ViewModels.SimScenarioVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisualizationWeb.Models.IRepo;

namespace VisualizationWeb.Models.Repo
{
    public class SimulationRepository : ISimulationRepository
    {
        private readonly ApplicationDbContext _context;

        public SimulationRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task CreatePosition(SimPositionCreateAndEditViewModel position)
        {
            throw new NotImplementedException();
        }

        public async Task CreateScenario(SimScenarioCreateAndEditViewModel scenario)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SimPositionIndexViewModel>> GetSimPositionIndex()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SimScenarioDetailsViewModel>> GetSimScenarioDetails()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SimScenarioIndexViewModel>> GetSimScenarioIndex()
        {
            throw new NotImplementedException();
        }

        public async Task RemovePosition(int positionID)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveScenario(int scenarioID)
        {
            throw new NotImplementedException();
        }
    }
}