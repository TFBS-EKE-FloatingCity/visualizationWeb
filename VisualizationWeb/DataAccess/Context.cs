using Core;
using Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DataAccess
{
   public class Context : IdentityDbContext<ApplicationUser>
   {
      public virtual DbSet<Setting> Settings { get; set; }

      public virtual DbSet<SimScenario> SimScenarios { get; set; }

      public virtual DbSet<SimPosition> SimPositions { get; set; }

      public virtual DbSet<CityData> CityDatas { get; set; }

      public virtual DbSet<CityDataHead> CityDataHeads { get; set; }

      public Context() : base("DefaultConnection", throwIfV1Schema: false) { }

      public static Context Create()
      {
         return new Context();
      }
   }
}