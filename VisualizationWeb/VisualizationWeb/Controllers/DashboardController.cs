using Application;
using Application.Services;
using Core.Entities;
using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.ViewModel;
using SelListItem = UI.ViewModel.SelectListItem;

namespace UI.Controllers
{
   public class DashboardController : Controller
   {

      private static SimulationStart _currentScenario;
      private readonly ScenarioService _service;
      private readonly SettingsRepository _settings;

      public DashboardController()
      {
         _service = new ScenarioService();
         ViewBag.ActiveNav = "dashboard";
         _settings = new SettingsRepository();
      }

      // GET: Dashboard
      public ActionResult Index()
      {
         return View("../Dashboard");
      }

      public string GetIPFromSettings()
      {
         return _settings.GetSimulationSettings().browserConnectionString;
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
      public async Task<ActionResult> StartSimulation([Bind(Include = "Duration, SimScenarioID, ScenarioSelectList")] SimulationStart vm)
      {
         var simScenarioID = await _service.GetScenarioByIdAsync(vm.SimScenarioID);
         _currentScenario = vm;
         simScenarioID.SimPositions = new List<SimPosition>(await _service.GetPositionsForScenarioAsync(vm.SimScenarioID));

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
         var vm = new SimulationStart();
         var scenarios = await _service.GetScenariosWithPositionsAsync();
         vm.ScenarioSelectList = new SelectList(
            scenarios.Select(x => new SelListItem
            {
               DisplayMember = x.Title,
               ValueMember = x.SimScenarioID
            }), 
            "ValueMember", 
            "DisplayMember"
            );

         return PartialView("../Charts/Partials/_StartSimulationPartial", vm);
      }

      public string GetSimulationTitle()
      {
         if (_currentScenario is null) return "No Simulation started";
         ViewBag.Started = true;
         return _service.GetScenarioTitle(_currentScenario.SimScenarioID);
      }
   }
}