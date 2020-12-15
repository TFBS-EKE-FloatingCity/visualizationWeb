﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simulation.Library.Models;

namespace VisualizationWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<SensorData> SensorDatas { get; set; }
        public virtual DbSet<SimData> SimDatas { get; set; }
        public virtual DbSet<SimType> SimTypes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SimulationHistory> SimulationHistories { get; set; }
        public virtual DbSet<SimScenario> SimScenarios { get; set; }
        public virtual DbSet<SimPosition> SimPositions { get; set; }

        public virtual DbSet<CityData> CityDatas { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}