using Core.Entities;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
   public class SettingsRepository : Repository<Setting>, ISettingsRepository
   {
      public Setting GetSimulationSettings()
      {
         return _context.Settings.FirstOrDefault() ?? new Setting
         {
            SettingID = 1,
            WindMax = 0,
            SunMax = 0,
            ConsumptionMax = 0
         };
      }

      /// <returns>True if connectionstring changed, false if not</returns>
      public async Task<bool> SaveSettingsAsync(Setting setting)
      {
         _context.Settings.AddOrUpdate(setting);
         await _context.SaveChangesAsync();

         return GetSimulationSettings().rbPiConnectionString != setting.rbPiConnectionString;
      }
   }
}
