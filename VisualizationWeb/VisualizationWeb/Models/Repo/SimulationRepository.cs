using Simulation.Library.Models;
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
            if(position != null)
            {
                _context.SimPositions.Add(new SimPosition
                {
                    SunValue = position.SunValue,
                    WindValue = position.WindValue,
                    EnergyBalanceValue = position.EnergyBalanceValue,
                    DateRegistered = position.DateRegistered,
                    
                });
            }
        }

        public async Task CreateScenario(SimScenarioCreateAndEditViewModel scenario)
        {
            if(scenario != null)
            {
                _context.SimScenarios.Add(new SimScenario
                {
                    Title = scenario.Title,
                    TimeFactor = scenario.TimeFactor,
                    Notes = scenario.Notes,
                    //TODO startdate/enddate
                });
            }
        }

        public async Task<IEnumerable<SimPositionIndexViewModel>> GetSimPositionIndex()
        {
            var pos = _context.SimPositions
        }

        public async Task<IEnumerable<SimScenarioDetailsViewModel>> GetSimScenarioDetails(int simScenarioID)
        {
            SimScenario simscenario = await _context.SimScenarios.FindAsync(simScenarioID);

            return new SimScenarioDetailsViewModel
            {
                SimScenarioID = simscenario.SimScenarioID,
                Title = simscenario.Title,
                Notes = simscenario.Notes,
                SimPositions = simscenario.SimPositions.Select(sp => new
                {
                    
                })
                
            }
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