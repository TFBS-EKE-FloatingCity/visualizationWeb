using Application;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using VisualizationWeb.ViewModel;

namespace VisualizationWeb.Controllers
{
   public class DashboardController : Controller
   {
      public ISimulationRepository SimulationRepository
      {
         get
         {
            if (_simRepo != null) return _simRepo;
            lock (_lock) return _simRepo ?? (_simRepo = new SimulationRepository(_context));
         }
      }

      private static SimStartViewModel _currentScenario;
      private ApplicationDbContext _context = new ApplicationDbContext();
      private object _lock = new object();
      private ISimulationRepository _simRepo;

      public DashboardController()
      {
         ViewBag.ActiveNav = "dashboard";
      }

      // GET: Dashboard
      public ActionResult Index()
      {
         return View("../Dashboard");
      }

      public string GetIPFromSettings()
      {
         return SimulationRepository.GetSimulationSetting().browserConnectionString;
      }

      public ActionResult Open3DModel()
      {
         return View("../Charts/ViewCityRotationChart");
      }

      public ActionResult RequestReconnect()
      {
         Mediator.RestartWebsocketClient();
         Mediator.StopSimulation();
         return View("../Dashboard");
      }

      [Authorize(Roles = "Admin, Simulant")]
      public async Task<ActionResult> StartSimulation([Bind(Include = "Duration, SimScenarioID")] SimStartViewModel vm)
      {
         var simScenarioID = await SimulationRepository.GetSimScenarioByID(vm.SimScenarioID);
         _currentScenario = vm;
         simScenarioID.SimPositions = new List<SimPosition>(await SimulationRepository.GetSimPositionsByID(vm.SimScenarioID));

         try
         {
            Mediator.StartSimulation(simScenarioID, vm.Duration);
         }
         catch (Exception ex)
         {
            TempData["StartSimulationError"] = ex.Message;
         }

         return RedirectToAction("../Dashboard");
      }

      //[Authorize(Roles = "Admin, Simulant")]
      public async Task<ActionResult> PartialSimulationStart()
      {
         ViewBag.SimScenarioID = new SelectList(
            await SimulationRepository.SimScenarioSelect(), 
            "ValueMember", 
            "DisplayMember"
            );

         return PartialView("../Charts/Partials/_StartSimulationPartial", new SimStartViewModel());
      }

      public string GetSimulationTitle()
      {
         if (_currentScenario is null) return "No Simulation started";
         ViewBag.Started = true;
         return SimulationRepository.GetSimulationTitle(_currentScenario.SimScenarioID);
      }
   }
}