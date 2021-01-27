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

            #region Adding User Roles & Administrator
           
            //Check if Roles already exists in the DB
            var roleAdmin = (from role in context.Roles
                             where role.Name == "Admin"
                             select role).FirstOrDefault();

            var roleSimulant = (from role in context.Roles
                                where role.Name == "Simulant"
                                select role).FirstOrDefault();

            //if both or only one not exists then the roles will be generated
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

            //Check if the Administrator - User exists
            var admin = (from user in context.Users
                            where user.UserName == "administrator@admin.at"
                         select user).FirstOrDefault();
            
            //If not the User will be added to the DB 
            if (admin == null)
            {

                var adminUser = new ApplicationUser
                {
                    UserName = "administrator@admin.at",
                    PasswordHash = "AIDBmmlD/Z17xlTYM59IPARX8HtdL/8kWnakl7DoYDEWzzwfnC6sPesKeD5aTTjnfQ==",
                    Email = "administrator@admin.at",
                    SecurityStamp = "8c1a36bb-7cc8-4924-8b1a-4f76215f3ccb"
                };
                context.Users.AddOrUpdate(adminUser);
                context.SaveChanges();
                //Add the new generated User to the 'Admin' Role
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.AddToRole(adminUser.Id, "Admin");

            }
            #endregion   

            context.SaveChanges();
        }
    }
}
