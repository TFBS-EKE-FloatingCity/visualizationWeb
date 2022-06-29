using Application.Services;
using Application.Services.Interfaces;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.ViewModel;

namespace UI.Controllers
{
   [Authorize(Roles = "Admin")]
   public class SimulationsController : Controller
   {

      private IScenarioService _service;

      public SimulationsController()
      {
         ViewBag.ActiveNav = "simulation";
         _service = new ScenarioService();
      }

      /// <summary>
      ///   Gets All SimScenarios for the Index View.
      /// </summary>
      public async Task<ActionResult> Index()
      {
         var scenarios = await _service.GetScenariosWithPositionsAsync();

         var vm = scenarios.Select(x => new SimScenarioIndex
         {
            Title = x.Title,
            isChecked = false,
            SimScenarioID = x.SimScenarioID,
            SimPositions = x.SimPositions.Select(y => new SimPositionIndex
            {
               EnergyConsumptionValue = y.EnergyConsumptionValue,
               SimPositionID = y.SimPositionID,
               SunValue = y.SunValue,
               TimeRegistered = y.TimeRegistered,
               WindValue = y.WindValue
            })
         });

         return View(vm);
      }

      /// <summary>
      ///   Gets All SimPositions for the given simScenarioID. Shows Positions in
      ///   PartialPositionIndex View
      /// </summary>
      /// <param name="id"> The selected SimScenario </param>
      public async Task<ActionResult> PartialPositionIndex(int? id)
      {
         if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         var positions = await _service.GetPositionsForScenarioAsync(id.Value);
         var vm = positions.Select(x => new SimPositionIndex
         {
            EnergyConsumptionValue = x.EnergyConsumptionValue,
            SimPositionID = x.SimPositionID,
            SunValue = x.SunValue,
            TimeRegistered = x.TimeRegistered,
            WindValue = x.WindValue
         });

         return View(vm);
      }

      /// <summary>
      ///   Gets SimScenario for the given simScenarioID. Shows SimScenario in Details View
      ///   SimPositions for the Scenario are Included
      /// </summary>
      /// <param name="id"> The selected SimScenario </param>
      public async Task<ActionResult> Details(int? id)
      {
         if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         var scenario = await _service.GetScenarioByIdAsync(id.Value);

         SimScenarioDetails vm = new SimScenarioDetails
         {
            Notes = scenario.Notes,
            SimScenarioID = scenario.SimScenarioID,
            Title = scenario.Title,
            SimPositions = (await _service.GetPositionsForScenarioAsync(scenario.SimScenarioID))
               .Select(x => new SimPositionIndex
               {
                  EnergyConsumptionValue = x.EnergyConsumptionValue,
                  SimPositionID = x.SimPositionID,
                  SunValue = x.SunValue,
                  WindValue = x.WindValue,
                  TimeRegistered = x.TimeRegistered
               })
         };

         return View(vm);
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
            await _service.CreateScenarioAsync(new SimScenario
            {
               IsSimulationRunning = false,
               Notes = vm.Notes,
               Title = vm.Title
            });

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
            //calculate new time registered
            var positions = await _service.GetPositionsForScenarioAsync(vm.SimScenarioID);
            if (positions.FirstOrDefault() != null)
            {
               var date = positions.First().TimeRegistered.Date;
               var time = vm.TimeRegistered.TimeOfDay;
               vm.TimeRegistered = date + time;
            }

            await _service.CreatePositionAsync(new SimPosition
            {
               EnergyConsumptionValue = vm.EnergyConsumptionValue,
               SimScenarioID = vm.SimScenarioID,
               SunValue = vm.SunValue,
               WindValue = vm.WindValue,
               TimeRegistered = vm.TimeRegistered
            });

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

         await _service.DeletePositionAsync(simPositionId.Value);

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

         await _service.DeleteScenarioAsync(simScenarioId.Value);

         return RedirectToAction("Index");
      }
   }
}