using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VisualizationWeb.Models;

namespace VisualizationWeb.Controllers
{
    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Settings
        public ActionResult Index()
        {
            Setting setting = db.Settings.FirstOrDefault();

            if (setting == null)
            {
                setting = new Setting
                {
                    SunActive = true,
                    WindActive = true,
                    ConsumptionActive = true,
                    SunMax = 10000000.0,
                    WindMax = 100000.0,
                    ConsumptionMax = 5000000.0
                };
            }

            return View(setting);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = db.Settings.FirstOrDefault();
            if (setting == null)
            {
                setting = new Setting
                {
                    SunActive = true,
                    WindActive = true,
                    ConsumptionActive = true,
                    SunMax = 10000000.0,
                    WindMax = 100000.0,
                    ConsumptionMax = 5000000.0
                };
            }
            return View(setting);
        }

        // POST: Settings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SettingID,WindMax,SunMax,ConsumptionMax,WindActive,SunActive,ConsumptionActive")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
