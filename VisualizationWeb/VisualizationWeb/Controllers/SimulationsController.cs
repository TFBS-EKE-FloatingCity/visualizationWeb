using Simulation.Library.Models;
using Simulation.Library.ViewModels.SimPositionVM;
using Simulation.Library.ViewModels.SimScenarioVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisualizationWeb.Models;
using VisualizationWeb.Models.IRepo;
using VisualizationWeb.Models.Repo;

namespace VisualizationWeb.Controllers
{
    public class SimulationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private object lobject = new object();
        private SimStartViewModel _currentScenario;

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

        // GET: Simulations
        public async Task<ActionResult> Index()
        {
            return View(await SimulationRepository.GetSimScenarioIndex());
        }

        public async Task<ActionResult> PartialPositionIndex(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(await SimulationRepository.GetSimPositionIndex(id.Value));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SimScenarioDetailsViewModel scenario = await SimulationRepository.GetSimScenarioDetails(id.Value);
            scenario.SimPositions = await SimulationRepository.GetSimPositionIndex(id.Value);
            return View(scenario);
        }

        public ActionResult Create()
        {
            return View(new SimScenarioCreateAndEditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SimScenarioID, Title, TimeFactor, Notes")] SimScenarioCreateAndEditViewModel vm)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                await SimulationRepository.CreateScenario(vm);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> PartialPositionCreate([Bind(Include = "SimPositionID, SunValue, WindValue, EnergyConsumptionValue, TimeRegistered, SimScenarioID")] SimPositionCreateAndEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await SimulationRepository.CreatePosition(vm);
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{vm.SimScenarioID}");
            }

            return View();
        }

        public async Task<ActionResult> RemoveSimPosition(int? simPositionId, int? simScenarioId)
        {
            if (!simPositionId.HasValue && !simScenarioId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await SimulationRepository.RemovePosition(simPositionId.Value);
            await db.SaveChangesAsync();

            return RedirectToAction($"Details/{simScenarioId}");
        }

        public async Task<ActionResult> RemoveSimScenario(int? simScenarioId)
        {
            if (!simScenarioId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await SimulationRepository.RemoveScenario(simScenarioId.Value);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> StartSimulation([Bind(Include = "Duration, SimScenarioID")] SimStartViewModel vm)
        {
            _currentScenario = vm;
            Helpers.SingletonHolder.StartSimulation(await SimulationRepository.GetSimScenarioByID(vm.SimScenarioID), vm.Duration);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> PartialSimulationStart()
        {
            SimStartViewModel vm = new SimStartViewModel();
            ViewBag.SimScenarioID = new SelectList(await SimulationRepository.SimScenarioSelect(), "ValueMember", "DisplayMember");

            return PartialView("PartialViews/PartialSimulationStart", vm);
        }

        public string GetSimulationTitle()
        {
            if (_currentScenario is null)
            {
                return "No Simulation started";
            }
            return SimulationRepository.GetSimulationTitle(_currentScenario.SimScenarioID);
        }
    }
}