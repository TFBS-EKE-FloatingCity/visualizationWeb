using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
   public class SimulationRepository : ISimulationRepository
   {
      private readonly ApplicationDbContext _context;

      public SimulationRepository(ApplicationDbContext context)
      {
         _context = context ?? throw new ArgumentNullException(nameof(context));
      }

      public async Task CreatePosition(SimPositionCreateAndEditViewModel position)
      {
         if (position != null)
         {
            var positions = await GetSimPositionsByID(position.SimScenarioID);
            if (positions?.FirstOrDefault() != null)
            {
               var date = positions.First().TimeRegistered.Date;
               var time = position.TimeRegistered.TimeOfDay;
               position.TimeRegistered = date + time;
            }

            _context.SimPositions.Add(new SimPosition
            {
               SunValue = position.SunValue,
               WindValue = position.WindValue,
               EnergyConsumptionValue = position.EnergyConsumptionValue,
               TimeRegistered = position.TimeRegistered,
               SimScenarioID = position.SimScenarioID
            });
         }
      }

      public async Task CreateScenario(SimScenarioCreateAndEditViewModel scenario)
      {
         if (scenario == null) return;

         _context.SimScenarios.Add(new SimScenario
         {
            Title = scenario.Title,
            Notes = scenario.Notes
         });
      }

      public async Task<IEnumerable<SimPositionBindingViewModel>> GetSimPositionBindingList(int simScenarioID)
      {
         var pos = from c in _context.SimPositions
                   orderby c.TimeRegistered
                   where c.SimScenarioID == simScenarioID
                   select new SimPositionBindingViewModel
                   {
                      SimPositionID = c.SimPositionID,
                      SunValue = c.SunValue,
                      WindValue = c.WindValue,
                      EnergyConsumptionValue = c.EnergyConsumptionValue,
                      TimeRegistered = c.TimeRegistered,
                   };
         return await pos.ToListAsync();
      }

      public async Task<IEnumerable<SimPositionIndexViewModel>> GetSimPositionIndex(int simScenarioID)
      {
         var pos = from c in _context.SimPositions
                   orderby c.TimeRegistered
                   where c.SimScenarioID == simScenarioID
                   select new SimPositionIndexViewModel
                   {
                      SimPositionID = c.SimPositionID,
                      SunValue = c.SunValue,
                      WindValue = c.WindValue,
                      EnergyConsumptionValue = c.EnergyConsumptionValue,
                      TimeRegistered = c.TimeRegistered,
                   };
         return await pos.ToListAsync();
      }

      public async Task<IEnumerable<SimPosition>> GetSimPositionsByID(int id)
      {
         return await _context.SimPositions.Where(x => x.SimScenarioID == id).ToListAsync();
      }

      public async Task<SimScenario> GetSimScenarioByID(int simScenarioID)
      {
         return await _context.SimScenarios.FindAsync(simScenarioID);
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
               EnergyConsumptionValue = sp.EnergyConsumptionValue,
               TimeRegistered = sp.TimeRegistered,
            })
         };
      }

      public async Task<IEnumerable<SimScenarioIndexViewModel>> GetSimScenarioIndex()
      {
         return await _context.SimScenarios.Include(x => x.SimPositions)
             .Select(sc => new SimScenarioIndexViewModel
             {
                SimScenarioID = sc.SimScenarioID,
                Title = sc.Title,
                SimPositions = sc.SimPositions.Select(sp => new SimPositionIndexViewModel
                {
                   SimPositionID = sp.SimPositionID,
                   SunValue = sp.SunValue,
                   WindValue = sp.WindValue,
                   EnergyConsumptionValue = sp.EnergyConsumptionValue,
                   TimeRegistered = sp.TimeRegistered,
                })
             }).ToListAsync();
      }

      public async Task RemovePosition(int positionID)
      {
         _context.SimPositions.Remove(await _context.SimPositions.FindAsync(positionID));
      }

      public async Task RemoveScenario(int scenarioID)
      {
         _context.SimScenarios.Remove(await _context.SimScenarios.FindAsync(scenarioID));
      }

      public async Task<IList<SelectListItemVM>> SimScenarioSelect()
      {
         var select = from c in _context.SimScenarios
                      orderby c.Title
                      select new SelectListItemVM
                      {
                         ValueMember = c.SimScenarioID,
                         DisplayMember = c.Title
                      };
         return await select.ToListAsync();
      }

      public Setting GetSimulationSetting()
      {
         Setting setting = _context.Settings.FirstOrDefault();
         if (setting != null) return setting;

         return new Setting
         {
            SettingID = 1,
            WindMax = 0,
            SunMax = 0,
            ConsumptionMax = 0
         };
      }
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="setting"></param>
      /// <returns>True if connectionstring changed, false if not</returns>
      public async Task<bool> SaveSetting(Setting setting)
      {
         //if ()
         //{
         //   Mediator.RestartWebsocketClient();
         //}

         _context.Settings.AddOrUpdate(setting);
         await _context.SaveChangesAsync();

         return GetSimulationSetting().rbPiConnectionString != setting.rbPiConnectionString;
      }

      public string GetSimulationTitle(int simScenarionID)
      {
         return (from c in _context.SimScenarios
                 orderby c.Title
                 where c.SimScenarioID == simScenarionID
                 select c.Title).Single();
      }
   }
}