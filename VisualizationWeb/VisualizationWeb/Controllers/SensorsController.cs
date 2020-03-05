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
    [Authorize (Roles = "Admin")]
    public class SensorsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        public SensorsController()
        {
            ViewBag.ActiveNav = "sensor";
        }

        // GET: Sensors
        public ActionResult Index()
        {
            return View(db.Sensors.ToList());
        }

        // GET: Sensors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sensors/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SensorID,Title,Notes,Factor,Einheiten,SCode,Prefix")] Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                db.Sensors.Add(sensor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sensor);
        }

        // GET: Sensors/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return HttpNotFound();
            }
            return View(sensor);
        }

        // POST: Sensors/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SensorID,Title,Notes,Factor,Einheiten,SCode,Prefix")] Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sensor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sensor);
        }

        // GET: Sensors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return HttpNotFound();
            }
            return View(sensor);
        }

        // POST: Sensors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sensor sensor = db.Sensors.Find(id);
            db.Sensors.Remove(sensor);
            db.SaveChanges();
            return RedirectToAction("Index");
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
