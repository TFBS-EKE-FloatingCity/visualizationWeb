using DataAccess;
using DataAccess.Entities.ViewModel;
using DataAccess.Repositories;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UI.Controllers
{
   [Authorize(Roles = "Admin")]
   public class SimulationsController : Controller
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

      /// <summary>
      ///   Initialise Repository only if needed Thread save (lobject)
      /// </summary>
      private ISimulationRepository _simRepo;

      public SimulationsController()
      {
         ViewBag.ActiveNav = "simulation";
      }

      /// <summary>
      ///   Gets All SimScenarios for the Index View.
      /// </summary>
      public async Task<ActionResult> Index()
      {
         return View(await SimulationRepository.GetAllSimScenarioIndices());
      }

      /// <summary>
      ///   Gets All SimPositions for the given simScenarioID. Shows Positions in
      ///   PartialPositionIndex View
      /// </summary>
      /// <param name="id"> The selected SimScenario </param>
      public async Task<ActionResult> PartialPositionIndex(int? id)
      {
         if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         return View(await SimulationRepository.GetSimPositionIndex(id.Value));
      }

      /// <summary>
      ///   Gets SimScenario for the given simScenarioID. Shows SimScenario in Details View
      ///   SimPositions for the Scenario are Included
      /// </summary>
      /// <param name="id"> The selected SimScenario </param>
      public async Task<ActionResult> Details(int? id)
      {
         if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         SimScenarioDetails scenario = await SimulationRepository.GetSimScenarioDetails(id.Value);
         var test = await SimulationRepository.GetSimPositionIndex(id.Value);
         scenario.SimPositions = test.OrderBy(x => x.TimeRegistered.TimeOfDay);
         return View(scenario);
      }

      /// <summary>
      ///   Opens Scenario Create View
      /// </summary>
      public ActionResult Create()
      {
         return View(new SimScenarioCreateAndEdit());
      }

      /// <summary>
      ///   Creates SimScenario After add redirect back to Scenario Index
      /// </summary>
      /// <param name="vm"> Newly created SimScenario </param>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Create([Bind(Include = "SimScenarioID, Title, TimeFactor, Notes")] SimScenarioCreateAndEdit vm)
      {
         var errors = ModelState.Values.SelectMany(v => v.Errors);
         if (ModelState.IsValid)
         {
            await SimulationRepository.CreateScenario(vm);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
         }

         return View(vm);
      }

      /// <summary>
      ///   Opens PartialPositionCreate View Creates SimPosition After add redirect back to Scenario Detail
      /// </summary>
      /// <param name="vm"> Newly created SimPosition </param>
      [HttpPost]
      public async Task<ActionResult> PartialPositionCreate([Bind(Include = "SimPositionID, SunValue, WindValue, EnergyConsumptionValue, TimeRegistered, SimScenarioID")] SimPositionCreateAndEdit vm)
      {
         if (ModelState.IsValid)
         {
            await SimulationRepository.CreatePosition(vm);
            await _context.SaveChangesAsync();
            return RedirectToAction($"Details/{vm.SimScenarioID}");
         }

         return PartialView("PartialViews/PartialPositionCreate", vm);
      }

      /// <summary>
      ///   Removes SimPosition After remove redirect back to Scenario Detail
      /// </summary>
      /// <param name="simPositionId"> SimPosition to delete </param>
      /// <param name="simScenarioId"> SimScenario to redirect </param>
      public async Task<ActionResult> RemoveSimPosition(int? simPositionId, int? simScenarioId)
      {
         if (!simPositionId.HasValue && !simScenarioId.HasValue)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         await SimulationRepository.DeletePosition(simPositionId.Value);
         await _context.SaveChangesAsync();

         return RedirectToAction($"Details/{simScenarioId}");
      }

      /// <summary>
      ///   Removes SimScenario After remove redirect back to Scenario Index
      /// </summary>
      /// <param name="simScenarioId"> SimScenario to delete </param>
      public async Task<ActionResult> RemoveSimScenario(int? simScenarioId)
      {
         if (!simScenarioId.HasValue)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         await SimulationRepository.DeleteScenario(simScenarioId.Value);
         await _context.SaveChangesAsync();

         return RedirectToAction("Index");
      }
   }
}