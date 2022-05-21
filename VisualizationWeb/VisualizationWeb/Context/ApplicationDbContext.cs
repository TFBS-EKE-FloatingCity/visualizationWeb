using Microsoft.AspNet.Identity.EntityFramework;
using Simulation.Library.Models;
using System.Data.Entity;
using VisualizationWeb.Models;

namespace VisualizationWeb.Context
{
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
   {
      public virtual DbSet<Setting> Settings { get; set; }

      public virtual DbSet<SimScenario> SimScenarios { get; set; }

      public virtual DbSet<SimPosition> SimPositions { get; set; }

      public virtual DbSet<CityData> CityDatas { get; set; }

      public virtual DbSet<CityDataHead> CityDataHeads { get; set; }

      public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }

      public static ApplicationDbContext Create()
      {
         return new ApplicationDbContext();
      }
   }
}