using Application.Services.Interfaces;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
   /// <summary>
   /// A Service responsible for SimScenarios and their SimPositions
   /// </summary>
   public class ScenarioService : IScenarioService
   {
      private readonly SimPositionRepository _posRepo;
      private readonly SimScenarioRepository _scenRepo;

      public ScenarioService()
      {
         _posRepo = new SimPositionRepository();
         _scenRepo = new SimScenarioRepository();
      }

      public async Task<SimPosition> CreatePositionAsync(SimPosition position)
      {
         return await _posRepo.AddAsync(position);
      }

      public async Task<SimScenario> CreateScenarioAsync(SimScenario scenario)
      {
         return await _scenRepo.AddAsync(scenario);
      }

      public async Task<bool> DeletePositionAsync(int id)
      {
         return await _posRepo.DeleteAsync(id);
      }

      public async Task<bool> DeleteScenarioAsync(int id)
      {
         return await _scenRepo.DeleteAsync(id);
      }

      public async Task<IEnumerable<SimPosition>> GetPositionsForScenarioAsync(int scenarioID)
      {
         return await _posRepo.GetAllWhereAsync(x => x.SimScenarioID == scenarioID);
      }

      public async Task<SimScenario> GetScenarioByIdAsync(int scenarioID)
      {
         return await _scenRepo.GetByIdAsync(scenarioID);
      }

      public async Task<List<SimScenario>> GetScenariosWithPositionsAsync()
      {
         return await _scenRepo.GetAllWithIncludingAsync(x => x.Include(nameof(SimScenario.SimPositions)));
      }

      public async Task<List<SimScenario>> GetScenariosAsync() => await _scenRepo.GetAllAsync();
   }
}
