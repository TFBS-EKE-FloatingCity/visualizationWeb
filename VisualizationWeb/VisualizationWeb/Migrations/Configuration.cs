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
                    Title = "Windr�der",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 14,
                    Title = "Windr�der",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                },
                new Sensor
                {
                    SensorID = 15,
                    Title = "Windr�der",
                    Einheiten = "mm",
                    Factor = 12,
                    SCode = 0,
                    Prefix = "",
                    Notes = ""
                }

            );

            context.SimTypes.AddOrUpdate(
                new SimType
                {
                    SimTypeID = 1,
                    Title = "Sun",
                    SimFactor = 10,
                    Notes = "Testing Sun",
                    StartTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Interval = new TimeSpan(01, 00, 00),
                    EndTime = new DateTime(2020, 03, 05, 08, 0, 0),
                },
                new SimType
                {
                    SimTypeID = 2,
                    Title = "Wind",
                    SimFactor = 10,
                    Notes = "Testing Wind",
                    StartTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Interval = new TimeSpan(00, 30, 00),
                    EndTime = new DateTime(2020, 03, 05, 08, 0, 0),
                },
                new SimType
                {
                    SimTypeID = 3,
                    Title = "Sun",
                    SimFactor = 10,
                    Notes = "Testing Sun",
                    StartTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Interval = new TimeSpan(01, 00, 00),
                    EndTime = new DateTime(2020, 03, 05, 08, 0, 0),
                },
                new SimType
                {
                    SimTypeID = 4,
                    Title = "Wind",
                    SimFactor = 10,
                    Notes = "Testing Wind",
                    StartTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Interval = new TimeSpan(01, 00, 00),
                    EndTime = new DateTime(2020, 03, 05, 08, 0, 0),
                }
            );


            context.SimDatas.AddOrUpdate(
                new SimData
                {
                    SimTypeID = 1,
                    SimDataID = 1,
                    RealTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    SimTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Wind = 10,
                    Sun = 10,
                    Consumption = 70,
                },
                new SimData
                {
                    SimTypeID = 1,
                    SimDataID = 2,
                    RealTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    SimTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Wind = 20,
                    Sun = 25,
                    Consumption = 65,
                },
                new SimData
                {
                    SimTypeID = 1,
                    SimDataID = 3,
                    RealTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    SimTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Wind = 30,
                    Sun = 40,
                    Consumption = 50,
                },
                new SimData
                {
                    SimTypeID = 1,
                    SimDataID = 4,
                    RealTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    SimTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Wind = 30,
                    Sun = 45,
                    Consumption = 45,
                },
                new SimData
                {
                    SimTypeID = 1,
                    SimDataID = 5,
                    RealTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    SimTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Wind = 35,
                    Sun = 65,
                    Consumption = 60,
                },
                new SimData
                {
                    SimTypeID = 1,
                    SimDataID = 6,
                    RealTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    SimTime = new DateTime(2020, 03, 05, 08, 0, 0),
                    Wind = 30,
                    Sun = 70,
                    Consumption = 70,
                }
            );

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
