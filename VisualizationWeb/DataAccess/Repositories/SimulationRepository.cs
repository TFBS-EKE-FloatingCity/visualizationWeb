using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{

   //SOLVE AND REMOVE ME
   public class SimulationRepository
   {
      private readonly Context _context;

      public SimulationRepository(Context context)
      {
         _context = context ?? throw new ArgumentNullException(nameof(context));
      }

      public async Task<IEnumerable<SimPositionBinding>> GetSimPositionBindingList(int simScenarioID)
      {
         var positions = await _context.SimPositions
            .Where(x => x.SimScenarioID == simScenarioID)
            .OrderBy(x => x.TimeRegistered)
            .ToListAsync();

         return positions.Select(x => new SimPositionBinding
         {
            SimPositionID = x.SimPositionID,
            SunValue = x.SunValue,
            WindValue = x.WindValue,
            EnergyConsumptionValue = x.EnergyConsumptionValue,
            TimeRegistered = x.TimeRegistered,
         });
      }

      public async Task<IList<SelectListItem>> GetSimScenarioSelectList()
      {
         var select = from c in _context.SimScenarios
                      orderby c.Title
                      select new SelectListItem
                      {
                         ValueMember = c.SimScenarioID,
                         DisplayMember = c.Title
                      };
         return await select.ToListAsync();
      }
   }
}