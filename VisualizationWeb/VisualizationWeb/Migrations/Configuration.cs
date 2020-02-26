namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VisualizationWeb.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<VisualizationWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VisualizationWeb.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Sensors.AddOrUpdate(
                new Sensor
                {
                    SensorID = 1,
                    Title = "",
                    Einheiten = "",
                    Factor = 12,
                    SCode = "",
                    Prefix = "",
                    Notes = "alles was zählt"
                }
            );

            context.Roles.AddOrUpdate(
                r => r.Id,
                new IdentityRole
                {
                    Id  = "1",
                    Name = "Admin",
                }
                );
            context.Roles.AddOrUpdate(
                r => r.Id,
                new IdentityRole
                {
                    Id = "2",
                    Name = "Benutzer",
                }
                );
            context.Roles.AddOrUpdate(
               r => r.Id,
               new IdentityRole
               {
                   Id = "3",
                   Name = "Gast",
               }
               );
        }
       
        
    }
}
