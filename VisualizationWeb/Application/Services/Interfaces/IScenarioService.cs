using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
   public interface IScenarioService
   {
      Task<bool> DeleteScenarioAsync(int id);
      Task<bool> DeletePositionAsync(int id);
      Task<List<SimScenario>> GetScenariosWithPositionsAsync();
      Task<SimPosition> CreatePositionAsync(SimPosition position);
      Task<SimScenario> CreateScenarioAsync(SimScenario scenario);
      Task<IEnumerable<SimPosition>> GetPositionsForScenarioAsync(int scenarioID);
      Task<SimScenario> GetScenarioByIdAsync(int scenarioID);
      Task<List<SimScenario>> GetScenariosAsync();
      string GetScenarioTitle(int id);
   }
}
