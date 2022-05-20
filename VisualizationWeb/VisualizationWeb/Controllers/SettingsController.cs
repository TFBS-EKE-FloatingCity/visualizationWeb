using System.Threading.Tasks;
using System.Web.Mvc;
using VisualizationWeb.Context;
using VisualizationWeb.Helpers;
using VisualizationWeb.Models;
using VisualizationWeb.Models.Repository;

namespace VisualizationWeb.Controllers
{
   [Authorize(Roles = "Admin")]
   public class SettingsController : Controller
   {
      public ISimulationRepository SimulationRepository
      {
         get
         {
            if (_simulationRepository == null)
            {
               lock (_lock)
               {
                  if (_simulationRepository == null)
                  {
                     _simulationRepository = new SimulationRepository(_db);
                  }
               }
            }
            return _simulationRepository;
         }
      }

      private ApplicationDbContext _db = new ApplicationDbContext();
      private object _lock = new object();

      private ISimulationRepository _simulationRepository;

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
            _db.Dispose();
         }

         base.Dispose(disposing);
      }
   }
}