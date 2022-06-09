using Core;
using DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace UI
{
   // Configure the application user manager used in this application. UserManager is defined in
   // ASP.NET Identity and is used by the application.
   public class ApplicationUserManager : UserManager<ApplicationUser>
   {
      public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

      public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
      {
         var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
         manager.UserValidator = new UserValidator<ApplicationUser>(manager)
         {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
         };

         manager.PasswordValidator = new PasswordValidator
         {
            RequiredLength = 5,
            RequireNonLetterOrDigit = false,
            RequireDigit = false,
            RequireLowercase = false,
            RequireUppercase = false,
         };

         manager.UserLockoutEnabledByDefault = false;
         manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
         manager.MaxFailedAccessAttemptsBeforeLockout = 5;

         // Register two factor authentication providers. This application uses Phone and Emails as
         // a step of receiving a code for verifying the user You can write your own provider and
         // plug it in here.
         manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
         {
            MessageFormat = "Your security code is {0}"
         });
         manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
         {
            Subject = "Security Code",
            BodyFormat = "Your security code is {0}"
         });
         var dataProtectionProvider = options.DataProtectionProvider;
         if (dataProtectionProvider != null)
         {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
         }
         return manager;
      }
   }
}