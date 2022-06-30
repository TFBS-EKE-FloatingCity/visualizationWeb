using Application;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UI.Controllers
{
   [Authorize(Roles = "Admin")]
   public class SettingsController : Controller
   {
      private SettingsRepository _settings;

      public SettingsController(SettingsRepository settings)
      {
         ViewBag.ActiveNav = "settings";
         _settings = settings;
      }

      // GET: Settings
      public ActionResult Index()
      {
         return View(_settings.GetSimulationSettings());
      }

      // POST: Settings/Edit/
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Index([Bind(Include = "SettingID,WindMax,SunMax,ConsumptionMax,WindActive,SunActive,ConsumptionActive,rbPiConnectionString,browserConnectionString")] Setting setting)
      {
         if (ModelState.IsValid)
         {
            await _settings.SaveSettingsAsync(setting);
            Mediator.UpdateSimulationSettings(setting);

            return RedirectToAction("../Dashboard");
         }

         return View(setting);
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
      }
   }
}