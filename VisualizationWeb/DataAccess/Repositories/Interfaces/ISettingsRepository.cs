using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
   public interface ISettingsRepository : IRepository<Setting>
   {
      Setting GetSimulationSettings();
      Task<bool> SaveSettingsAsync(Setting setting);
   }
}
