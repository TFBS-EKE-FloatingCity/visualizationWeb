using Simulation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private ISimulationRepository _priceListRepository;
        public ISimulationRepository PriceListRepository
        {
            get
            {
                if (_priceListRepository == null)
                {
                    lock (lobject)
                    {
                        if (_priceListRepository == null)
                        {
                            _priceListRepository = new SimulationRepository(db);
                        }
                    }
                }
                return _priceListRepository;
            }
        }

        // GET: Simulations
        public ActionResult Index()
        {
            return View(db.SimScenarios.ToList());
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new SimScenario { SimPositions = new List<SimPosition>()});
        }

        [HttpPost]
        public ActionResult AddPosition()
        {
            return View();
        }

        public ActionResult EditSimScenario()
        {
            return View();
        }

        public ActionResult EditSimPosition()
        {
            return View();
        }

        public ActionResult DeleteSimScenario()
        {
            return View();
        }

        public ActionResult DeleteSimPosition()
        {
            return View();
        }
    }
}