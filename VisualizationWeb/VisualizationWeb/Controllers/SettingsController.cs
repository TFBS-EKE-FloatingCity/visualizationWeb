using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisualizationWeb.Helpers;
using VisualizationWeb.Models;
using VisualizationWeb.Models.IRepo;
using VisualizationWeb.Models.Repo;
using VisualizationWeb.Models.ViewModel;

namespace VisualizationWeb.Controllers
{
    //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await SimulationRepository.GetSimulationSetting());
        }

        // POST: Settings/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index([Bind(Include = "SettingID,WindMax,SunMax,ConsumptionMax,WindActive,SunActive,ConsumptionActive")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                await SimulationRepository.SaveSetting(setting);
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
