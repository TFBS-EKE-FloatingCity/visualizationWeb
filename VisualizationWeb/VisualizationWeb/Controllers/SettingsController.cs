using System.Threading.Tasks;
using System.Web.Mvc;
using VisualizationWeb.Context;
using VisualizationWeb.Helpers;
using VisualizationWeb.Models;
using VisualizationWeb.Models.Repository;
using VisualizationWeb.Models.Repository;

namespace VisualizationWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private object lobject = new object();

        private ISimulationRepository _simulationRepository;
        public ISimulationRepository SimulationRepository
        {
            get
            {
                if (_simulationRepository == null)
                {
                    lock (lobject)
                    {
                        if (_simulationRepository == null)
                        {
                            _simulationRepository = new SimulationRepository(db);
                        }
                    }
                }
                return _simulationRepository;
            }
        }

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

                SingletonHolder.UpdateSimulationSettings(setting);

                return RedirectToAction("../Dashboard");
            }

            return View(setting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
