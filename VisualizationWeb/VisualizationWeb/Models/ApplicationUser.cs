using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VisualizationWeb.Models
{
   public class ApplicationUser : IdentityUser
   {
      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
      {
         return await manager.CreateIdentityAsync(this, Startup.CookieAuthenticationType);
      }
   }
}