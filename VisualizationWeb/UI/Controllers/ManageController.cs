using Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.ViewModel.ManageViewModels;

namespace UI.Controllers
{
   [Authorize]
   public class ManageController : Controller
   {
      // Used for XSRF protection when adding external logins
      private const string XsrfKey = "XsrfId";

      public ApplicationSignInManager SignInManager
      {
         get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
         private set => _signInManager = value;
      }

      public ApplicationUserManager UserManager
      {
         get =>_userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
         private set => _userManager = value;
      }

      private ApplicationSignInManager _signInManager;
      private ApplicationUserManager _userManager;

      public ManageController()
      {
      }

      public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
      {
         UserManager = userManager;
         SignInManager = signInManager;
      }

      // GET: /Manage/Index
      public async Task<ActionResult> Index(ManageMessageId? message)
      {
         ViewBag.StatusMessage =
             message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
             : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
             : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
             : message == ManageMessageId.Error ? "An error has occurred."
             : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
             : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
             : "";

         var userId = User.Identity.GetUserId();
         var model = new IndexViewModel
         {
            HasPassword = HasPassword(),
            PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
            TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
            Logins = await UserManager.GetLoginsAsync(userId),
            BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
         };
         return View(model);
      }

      // POST: /Manage/RemoveLogin
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
      {
         ManageMessageId? message = ManageMessageId.Error;
         var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
         if (result.Succeeded)
         {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null) await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            message = ManageMessageId.RemoveLoginSuccess;
         }

         return RedirectToAction("ManageLogins", new { Message = message });
      }

      // GET: /Manage/AddPhoneNumber
      public ActionResult AddPhoneNumber()
      {
         return View();
      }

      // POST: /Manage/AddPhoneNumber
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel vm)
      {
         if (!ModelState.IsValid) return View(vm);

         // Generate the token and send it
         var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), vm.Number);
         if (UserManager.SmsService != null)
         {
            var message = new IdentityMessage
            {
               Destination = vm.Number,
               Body = "Your security code is: " + code
            };
            await UserManager.SmsService.SendAsync(message);
         }

         return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = vm.Number });
      }

      // POST: /Manage/EnableTwoFactorAuthentication
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> EnableTwoFactorAuthentication()
      {
         await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null) await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
         return RedirectToAction("Index", "Manage");
      }

      // POST: /Manage/DisableTwoFactorAuthentication
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> DisableTwoFactorAuthentication()
      {
         await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null) await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
         return RedirectToAction("Index", "Manage");
      }

      // GET: /Manage/VerifyPhoneNumber
      public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
      {
         var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
         // Send an SMS through the SMS provider to verify the phone number
         return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
      }

      // POST: /Manage/VerifyPhoneNumber
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel vm)
      {
         if (!ModelState.IsValid) return View(vm);

         var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), vm.PhoneNumber, vm.Code);
         if (!result.Succeeded)
         {
            ModelState.AddModelError("", "Failed to verify phone");
            return View(vm);
         }

         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null) await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

         return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
      }

      // POST: /Manage/RemovePhoneNumber
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> RemovePhoneNumber()
      {
         var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
         if (!result.Succeeded)
         {
            return RedirectToAction("Index", new { Message = ManageMessageId.Error });
         }
         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null)
         {
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
         }
         return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
      }

      // GET: /Manage/ChangePassword
      public ActionResult ChangePassword()
      {
         return View();
      }

      // POST: /Manage/ChangePassword
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ChangePassword(ChangePasswordViewModel vm)
      {
         if (!ModelState.IsValid) return View(vm);

         var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), vm.OldPassword, vm.NewPassword);
         if (!result.Succeeded)
         {
            AddErrors(result);
            return View(vm);
         }

         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null) await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

         return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
      }

      // GET: /Manage/SetPassword
      public ActionResult SetPassword()
      {
         return View();
      }

      // POST: /Manage/SetPassword
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> SetPassword(SetPasswordViewModel vm)
      {
         if (!ModelState.IsValid) return View(vm);

         var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), vm.NewPassword);
         if (!result.Succeeded)
         {
            AddErrors(result);
            return View(vm);
         }

         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null) await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

         return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
      }

      // GET: /Manage/ManageLogins
      public async Task<ActionResult> ManageLogins(ManageMessageId? message)
      {
         ViewBag.StatusMessage = message == ManageMessageId.RemoveLoginSuccess 
            ? "The external login was removed."
            : message == ManageMessageId.Error 
            ? "An error has occurred."
            : "";
         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user == null) return View("Error");

         var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
         var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
         ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
         return View(new ManageLoginsViewModel
         {
            CurrentLogins = userLogins,
            OtherLogins = otherLogins
         });
      }

      // POST: /Manage/LinkLogin
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LinkLogin(string provider)
      {
         // Request a redirect to the external login provider to link a login for the current user
         return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
      }

      // GET: /Manage/LinkLoginCallback
      public async Task<ActionResult> LinkLoginCallback()
      {
         var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
         if (loginInfo == null) return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });

         var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);

         return result.Succeeded 
            ? RedirectToAction("ManageLogins") 
            : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing && _userManager != null)
         {
            _userManager.Dispose();
            _userManager = null;
         }

         base.Dispose(disposing);
      }

      private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

      public enum ManageMessageId
      {
         AddPhoneSuccess,
         ChangePasswordSuccess,
         SetTwoFactorSuccess,
         SetPasswordSuccess,
         RemoveLoginSuccess,
         RemovePhoneSuccess,
         Error
      }

      private void AddErrors(IdentityResult result)
      {
         foreach (var error in result.Errors)
         {
            ModelState.AddModelError("", error);
         }
      }

      private bool HasPassword()
      {
         var user = GetUser();
         return user != null && user.PasswordHash != null;
      }

      private bool HasPhoneNumber()
      {
         var user = GetUser();
         return user != null && user.PhoneNumber != null;
      }

      private ApplicationUser GetUser() => UserManager.FindById(User.Identity.GetUserId());
   }
}