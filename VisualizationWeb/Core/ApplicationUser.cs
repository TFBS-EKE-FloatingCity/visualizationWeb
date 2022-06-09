using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core
{
   public class ApplicationUser : IdentityUser
   {
      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
         UserManager<ApplicationUser> manager,
         string cookieAuthenticationType)
      {
         return await manager.CreateIdentityAsync(this, cookieAuthenticationType);
      }
   }
}