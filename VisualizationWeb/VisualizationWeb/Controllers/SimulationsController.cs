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
    [Authorize(Roles = "Admin")]
    public class SimulationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private object lobject = new object();

        /// <summary>
        /// Initialise Repository only if needed
        /// Thread save (lobject)
        /// </summary>
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

        /// <summary>
        /// Gets All SimScenarios for the Index View.
        /// </summary>
        public async Task<ActionResult> Index()
        {
            return View(await SimulationRepository.GetSimScenarioIndex());
        }

        /// <summary>
        /// Gets All SimPositions for the given simScenarioID.
        /// Shows Positions in PartialPositionIndex View
        /// </summary>
        /// <param name="id">The selected SimScenario</param>
        public async Task<ActionResult> PartialPositionIndex(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(await SimulationRepository.GetSimPositionIndex(id.Value));
        }

        /// <summary>
        /// Gets SimScenario for the given simScenarioID.
        /// Shows SimScenario in Details View
        /// SimPositions for the Scenario are Included
        /// </summary>
        /// <param name="id">The selected SimScenario</param>
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SimScenarioDetailsViewModel scenario = await SimulationRepository.GetSimScenarioDetails(id.Value);
            var test = await SimulationRepository.GetSimPositionIndex(id.Value);
            scenario.SimPositions = test.OrderBy(x => x.TimeRegistered.TimeOfDay);
            return View(scenario);
        }

        /// <summary>
        /// Opens Scenario Create View.
        /// </summary>
        public ActionResult Create()
        {
            return View(new SimScenarioCreateAndEditViewModel());
        }

        /// <summary>
        /// Creates SimScenario
        /// After add redirect back to Scenario Index
        /// </summary>
        /// <param name="vm">Newly created SimScenario</param>
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

        /// <summary>
        /// Opens PartialPositionCreate View
        /// Creates SimPosition
        /// After add redirect back to Scenario Detail
        /// </summary>
        /// <param name="vm">Newly created SimPosition</param>
        [HttpPost]
        public async Task<ActionResult> PartialPositionCreate([Bind(Include = "SimPositionID, SunValue, WindValue, EnergyConsumptionValue, TimeRegistered, SimScenarioID")] SimPositionCreateAndEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await SimulationRepository.CreatePosition(vm);
                await db.SaveChangesAsync();
                return RedirectToAction($"Details/{vm.SimScenarioID}");
            }

            return PartialView("PartialViews/PartialPositionCreate", vm);
        }

        /// <summary>
        /// Removes SimPosition
        /// After remove redirect back to Scenario Detail
        /// </summary>
        /// <param name="simPositionId">SimPosition to delete</param>
        /// <param name="simScenarioId">SimScenario to redirect</param>
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

        /// <summary>
        /// Removes SimScenario
        /// After remove redirect back to Scenario Index
        /// </summary>
        /// <param name="simScenarioId">SimScenario to delete</param>
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

        //STFR: Uncomment if you want to Start the Simulation in Simulation View directly!
        //public async Task<ActionResult> StartSimulation([Bind(Include = "Duration, SimScenarioID")] SimStartViewModel vm)
        //{
           
        //    Helpers.SingletonHolder.StartSimulation(await SimulationRepository.GetSimScenarioByID(vm.SimScenarioID), vm.Duration);

        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> PartialSimulationStart()
        //{
        //    SimStartViewModel vm = new SimStartViewModel();
        //    ViewBag.SimScenarioID = new SelectList(await SimulationRepository.SimScenarioSelect(), "ValueMember", "DisplayMember");

        //    return PartialView("PartialViews/PartialSimulationStart", vm);
        //}

       
    }
}