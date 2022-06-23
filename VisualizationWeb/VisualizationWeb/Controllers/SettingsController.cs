using Application;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UI.Controllers
{
   [Authorize(Roles = "Admin")]
   public class SettingsController : Controller
   {
      public ISimulationRepository SimulationRepository
      {
         get
         {
            if (_simRepo != null) return _simRepo;
            lock (_lock) return _simRepo ?? (_simRepo = new SimulationRepository(_context));
         }
      }

      private Context _context = new Context();
      private object _lock = new object();

      private ISimulationRepository _simRepo;

      public SettingsController()
      {
         ViewBag.ActiveNav = "settings";
      }

      // GET: Settings
      public ActionResult Index()
      {
         return View(SimulationRepository.GetSimulationSetting());
      }

      // POST: Settings/Edit/
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Index([Bind(Include = "SettingID,WindMax,SunMax,ConsumptionMax,WindActive,SunActive,ConsumptionActive,rbPiConnectionString,browserConnectionString")] Setting setting)
      {
         if (ModelState.IsValid)
         {
            await SimulationRepository.SaveSetting(setting);
            Mediator.UpdateSimulationSettings(setting);

            return RedirectToAction("../Dashboard");
         }

         return View(setting);
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            _context.Dispose();
         }

         base.Dispose(disposing);
      }
   }
}