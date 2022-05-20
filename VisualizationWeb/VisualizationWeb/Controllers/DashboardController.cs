using Simulation.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using VisualizationWeb.Context;
using VisualizationWeb.Models;
using VisualizationWeb.Models.Repository;
using VisualizationWeb.ViewModel.SimScenarioVM;

namespace VisualizationWeb.Controllers
{
   public class DashboardController : Controller
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

      //For the ProgressBar
      private static SimStartViewModel _currentScenario;

      private ApplicationDbContext _db = new ApplicationDbContext();
      private object _lock = new object();
      private ISimulationRepository _simulationRepository;

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
         SimulationRepository simrep = new SimulationRepository(_db);
         Setting setting = simrep.GetSimulationSetting();

         return setting.browserConnectionString;
      }

      public ActionResult Open3DModel()
      {
         return View("../Charts/ViewCityRotationChart");
      }

      [Authorize(Roles = "Admin, Simulant")]
      public async Task<ActionResult> StartSimulation([Bind(Include = "Duration, SimScenarioID")] SimStartViewModel vm)
      {
         var simScenarioID = await SimulationRepository.GetSimScenarioByID(vm.SimScenarioID);
         _currentScenario = vm;
         simScenarioID.SimPositions = new List<SimPosition>(await SimulationRepository.GetSimPositionsByID(vm.SimScenarioID));
         Helpers.Mediator.StartSimulation(simScenarioID, vm.Duration);

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