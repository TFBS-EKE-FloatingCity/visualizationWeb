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
                    Title = "Lagersensor",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 2,
                    Title = "Lagersensor",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 3,
                    Title = "Lagersensor",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 4,
                    Title = "Ultraschall",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 5,
                    Title = "Ultraschall",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 6,
                    Title = "Ultraschall",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 7,
                    Title = "Pumpen",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 8,
                    Title = "Pumpen",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 9,
                    Title = "Pumpen",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 10,
                    Title = "Generatoren",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 11,
                    Title = "Generatoren",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 12,
                    Title = "Generatoren",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 13,
                    Title = "Windräder",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 14,
                    Title = "Windräder",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 15,
                    Title = "Windräder",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                }

            );

            //context.Sensors.AddOrUpdate(
            //    new SimType
            //    {
            //        SimTypeID = 1,
            //        Title = "Sun",
            //        SimFactor = 10,
            //        Notes = "Testing Sun",
            //        StartTime = 07:00:00,
            //        Interval = 01:00:00,
            //        EndTime = 12:00:00,
            //    }
            //);

            context.Roles.AddOrUpdate(
                r => r.Id,
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Benutzer",
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Gast",
                }
            );

            //context.Users.AddOrUpdate(
            //    new ApplicationUser
            //    {
            //        Id = "1",
            //        UserName = "Admin",
            //        PasswordHash = "AOPYVSJfQbP9b64NySmU2NJyapRr8G8dmLrnsXkjumfMAr5UdyHmOCF5MaGa8x0hTg=="

            //    },
            //    new ApplicationUser
            //    {
            //        Id = "2",
            //        UserName = "Gast",
            //        PasswordHash = ""

            //    }
            //);

            context.Settings.AddOrUpdate(
                new Setting
                {
                    SettingID = 1,
                    SunActive = true,
                    WindActive = true,
                    ConsumptionActive = true,
                    SunMax = 00.00,
                    WindMax = 00.00,
                    ConsumptionMax = 00.00
                }
            );

            context.SaveChanges();
        }    
    }
}
