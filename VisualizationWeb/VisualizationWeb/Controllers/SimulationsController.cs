using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisualizationWeb.Models;

namespace VisualizationWeb.Controllers
{
    public class SimulationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Simulations
        public ActionResult Index()
        {
            return View(db.SimScenarios.ToList());
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult CreateSimScenario()
        {
            return View();
        }

        public ActionResult CreateSimPosition()
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