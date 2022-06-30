using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
   public interface IRepository<T> where T : class
   {
      Task<T> GetByIdAsync(int id);

      Task<List<T>> GetAllAsync();

      Task<bool> ExistsAsync(int id);

      Task<bool> DeleteAsync(int id);

      Task<T> AddAsync(T entity);
      
      Task<List<T>> GetAllWithIncludingAsync(Func<DbSet<T>, IQueryable<T>> includeFunc);
   }
}
