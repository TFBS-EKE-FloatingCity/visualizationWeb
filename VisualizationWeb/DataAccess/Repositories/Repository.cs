using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
   public class Repository<T> : IRepository<T> where T : class
   {
      protected readonly Context _context;

      public Repository()
      {
         _context = new Context();
      }

      public async Task<T> AddAsync(T entity)
      {
         if (entity == null) return null;

         _context.Set<T>().Add(entity);
         await _context.SaveChangesAsync();
         return entity;
      }

      public async Task<bool> DeleteAsync(int id)
      {
         if (await _context.Set<T>().FindAsync(id) is T entry)
         {
            _context.Set<T>().Remove(entry);
            await _context.SaveChangesAsync();
            return true;
         }

         return false;
      }

      public async Task<bool> ExistsAsync(int id)
      {
         return await _context.Set<T>().FindAsync(id) != null;
      }

      public async Task<List<T>> GetAllAsync()
      {
         return await _context.Set<T>().ToListAsync();
      }

      public async Task<List<T>> GetAllWhereAsync(Predicate<T> filter)
      {
         return await _context.Set<T>().Where(x => filter.Invoke(x)).ToListAsync();
      }

      public async Task<T> GetByIdAsync(int id)
      {
         return await _context.Set<T>().FindAsync(id);
      }

      public async Task<List<T>> GetAllWithIncludingAsync(Func<DbSet<T>, IQueryable<T>> includeFunc)
      {
         return await includeFunc.Invoke(_context.Set<T>()).ToListAsync();
      }
   }
}a
