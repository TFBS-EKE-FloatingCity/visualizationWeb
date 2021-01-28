using Simulation.Library.Models;
using Simulation.Library.ViewModels.SimScenarioVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisualizationWeb.Helpers;
using VisualizationWeb.Models;
using VisualizationWeb.Models.IRepo;
using VisualizationWeb.Models.Repo;

namespace VisualizationWeb.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private object lobject = new object();
        //For the ProgressBar
        private static SimStartViewModel _currentScenario;

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

        public DashboardController()
        {
            ViewBag.ActiveNav = "dashboard";            
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View("../Dashboard");
        }

        public ActionResult Open3DModell()
        {
            return View("../Charts/ViewCityRotationChart");
        }

        public async Task<ActionResult> StartSimulation([Bind(Include = "Duration, SimScenarioID")] SimStartViewModel vm)
        {            
            var simScenarioID = await SimulationRepository.GetSimScenarioByID(vm.SimScenarioID);
            _currentScenario = vm;
            simScenarioID.SimPositions = new List<SimPosition>(await SimulationRepository.GetSimPositionsByID(vm.SimScenarioID));
            Helpers.SingletonHolder.StartSimulation(simScenarioID, vm.Duration);

            return RedirectToAction("../Dashboard");
        }

        //[Authorize(Roles = "Admin, Simulant")]
        public async Task<ActionResult> PartialSimulationStart()
        {
            SimStartViewModel vm = new SimStartViewModel();
            ViewBag.SimScenarioID = new SelectList(await SimulationRepository.SimScenarioSelect(), "ValueMember", "DisplayMember");

            return PartialView("../Charts/Partials/_StartSimulationPartial", vm);
        }

        public string GetSimulationTitle()
        {            
            if (_currentScenario is null)
            {
                return "No Simulation started";
            }
            ViewBag.Started = true;
            return SimulationRepository.GetSimulationTitle(_currentScenario.SimScenarioID);
        }
    }
}