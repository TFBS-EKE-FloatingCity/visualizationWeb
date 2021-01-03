using Simulation.Library.Models;
using Simulation.Library.ViewModels.SimPositionVM;
using Simulation.Library.ViewModels.SimScenarioVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            if (position != null)
            {
                _context.SimPositions.Add(new SimPosition
                {
                    SunValue = position.SunValue,
                    WindValue = position.WindValue,
                    EnergyBalanceValue = position.EnergyBalanceValue,
                    DateRegistered = position.DateRegistered,
                    SimScenarioID = position.SimScenarioID
                });
            }
        }

        public async Task CreateScenario(SimScenarioCreateAndEditViewModel scenario)
        {
            if (scenario != null)
            {
                _context.SimScenarios.Add(new SimScenario
                {
                    Title = scenario.Title,
                    TimeFactor = scenario.TimeFactor,
                    Notes = scenario.Notes,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                });
            }
        }

        public async Task<IEnumerable<SimPositionBindingViewModel>> GetSimPositionBindingList(int simScenarioID)
        {
            var pos = from c in _context.SimPositions
                      orderby c.DateRegistered
                      where c.SimScenarioID == simScenarioID
                      select new SimPositionBindingViewModel
                      {
                          SimPositionID = c.SimPositionID,
                          SunValue = c.SunValue,
                          WindValue = c.WindValue,
                          EnergyBalanceValue = c.EnergyBalanceValue,
                          DateRegistered = c.DateRegistered,
                      };
            return await pos.ToListAsync();
        }

        public async Task<IEnumerable<SimPositionIndexViewModel>> GetSimPositionIndex(int simScenarioID)
        {
            var pos = from c in _context.SimPositions
                      orderby c.DateRegistered
                      where c.SimScenarioID == simScenarioID
                      select new SimPositionIndexViewModel
                      {
                          SimPositionID = c.SimPositionID,
                          SunValue = c.SunValue,
                          WindValue = c.WindValue,
                          EnergyBalanceValue = c.EnergyBalanceValue,
                          DateRegistered = c.DateRegistered,
                      };
            return await pos.ToListAsync();

        }

        public async Task<SimScenarioDetailsViewModel> GetSimScenarioDetails(int simScenarioID)
        {
            SimScenario simscenario = await _context.SimScenarios.FindAsync(simScenarioID);

            return new SimScenarioDetailsViewModel
            {
                SimScenarioID = simscenario.SimScenarioID,
                Title = simscenario.Title,
                Notes = simscenario.Notes,
                SimPositions = simscenario.SimPositions?.Select(sp => new SimPositionIndexViewModel
                {
                    SimPositionID = sp.SimPositionID,
                    SunValue = sp.SunValue,
                    WindValue = sp.WindValue,
                    EnergyBalanceValue = sp.EnergyBalanceValue,
                    DateRegistered = sp.DateRegistered,
                })
            };
        }

        public async Task<IEnumerable<SimScenarioIndexViewModel>> GetSimScenarioIndex()
        {
            var scen = from c in _context.SimScenarios
                       orderby c.Title
                       select new SimScenarioIndexViewModel
                       {
                           SimScenarioID = c.SimScenarioID,
                           Title = c.Title,
                           TimeFactor = c.TimeFactor,
                           StartDate = c.StartDate,
                           EndDate = c.EndDate,
                       };
            return await scen.ToListAsync();
        }


        public async Task RemovePosition(int positionID)
        {

            _context.SimPositions.Remove(await _context.SimPositions.FindAsync(positionID));
        }

        public async Task RemoveScenario(int scenarioID)
        {
            _context.SimScenarios.Remove(await _context.SimScenarios.FindAsync(scenarioID));
        }
    }
}
