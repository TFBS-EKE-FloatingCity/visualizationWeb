namespace VisualizationWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VisualizationWeb.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

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

            #region Adding User Roles
            if (context.Roles.Find("3") != null)
            {
                context.Roles.Remove(context.Roles.Find("3"));
            }

            if (context.Roles.Find("2") != null)
            {
                context.Roles.Remove(context.Roles.Find("2"));
            }

            if (context.Roles.Find("1") != null)
            {
                context.Roles.Remove(context.Roles.Find("1"));
            }

            var roleAdmin = (from role in context.Roles
                             where role.Name == "Admin"
                             select role).FirstOrDefault();

            var roleSimulant = (from role in context.Roles
                                where role.Name == "Simulant"
                                select role).FirstOrDefault();

            if (roleAdmin == null && roleSimulant == null)
            {
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
                         Name = "Simulant",
                     }
                );
            }
            else if (roleAdmin == null)
            {
                context.Roles.AddOrUpdate(
                    r => r.Id,
                    new IdentityRole
                    {
                        Id = "1",
                        Name = "Admin",
                    }
                 );
            }
            else if (roleSimulant == null)
            {
                context.Roles.AddOrUpdate(
                    r => r.Id,
                    new IdentityRole
                    {
                        Id = "2",
                        Name = "Simulant",
                    }
                );
            }

            #endregion

            //context.Users.AddOrUpdate(
            ////    new ApplicationUser
            ////    {
            ////        Id = "1",
            ////        UserName = "Admin",
            ////        PasswordHash = "AOPYVSJfQbP9b64NySmU2NJyapRr8G8dmLrnsXkjumfMAr5UdyHmOCF5MaGa8x0hTg=="

            ////    },
            //    new ApplicationUser
            //    {
            //        Id = "2",
            //        UserName = "Benutzer",
            //        PasswordHash = ""

            //    }
            //);        

            context.SaveChanges();
        }
    }
}
