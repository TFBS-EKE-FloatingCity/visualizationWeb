﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VisualizationWeb.Controllers
{
    public class DashboardController : Controller
    {
        public DashboardController()
        {
            ViewBag.ActiveNav = "dashboard";
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View("../Dashboard");
        }
    }
}