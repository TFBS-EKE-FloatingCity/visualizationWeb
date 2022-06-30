using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
   public interface ISimScenarioRepository : IRepository<SimScenario>
   {
      string GetScenarioTitle(int scenarioId);
   }
}
