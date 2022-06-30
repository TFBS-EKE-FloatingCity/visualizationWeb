using Core.Entities;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
   public class SimScenarioRepository : Repository<SimScenario>, ISimScenarioRepository
   {
      public string GetScenarioTitle(int scenarioId)
      {
         return _context.SimScenarios.FirstOrDefault(x => x.SimScenarioID == scenarioId)?.Title;
      }
   }
}
