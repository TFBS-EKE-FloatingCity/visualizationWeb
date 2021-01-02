﻿using Simulation.Library.Models;
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

        public async Task<ActionResult> PartialPositionIndex(int? simScenarioId)
        {
            if (!simScenarioId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(await SimulationRepository.GetSimPositionIndex(simScenarioId.Value));
        }

        public async Task<ActionResult> Details(int? simScenarioId)
        {
            if (!simScenarioId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SimScenarioDetailsViewModel scenario = await SimulationRepository.GetSimScenarioDetails(simScenarioId.Value);
            scenario.SimPositions = await SimulationRepository.GetSimPositionIndex(simScenarioId.Value);
            return View(scenario);
        }

        public async Task<ActionResult> Create()
        {
            return View();
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

        public async Task<ActionResult> PartialPositionCreate()
        {
            return View();
        }
    }
}