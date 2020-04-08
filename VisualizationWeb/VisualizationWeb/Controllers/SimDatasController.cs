using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VisualizationWeb.Models;

namespace VisualizationWeb.Controllers
{
    public class SimDatasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SimDatas
        public async Task<ActionResult> Index(string ReturnTo)
        {
            var simDatas = db.SimDatas.Include(s => s.SimType);
            return View(await simDatas.ToListAsync());
        }

        //// GET: SimDatas/Create
        //public async Task<ActionResult> Create(int? id)
        //{
        //    var simtype = await db.SimTypes.FindAsync(id);
            
        //    var simdata = new SimData
        //    {
        //        SimTypeID = simtype.SimTypeID,
        //        SimTime = simtype.StartTime
        //    };

        //    ViewBag.SimTypeID = id;
        //    ViewData["ReturnTo"] = "../SimTypes/Details/" + id.ToString();
        //    return View(simdata);
        //}

        // GET: SimDatas/Create
        public async Task<ActionResult> Create(int? id, int? dataID)
        {
            var simtype = await db.SimTypes.FindAsync(id);
            SimData simdata;
            DateTime newSimTime = simtype.StartTime;

            if (dataID != null)
            {
                var simDataPrev = await db.SimDatas.FindAsync(dataID);
                var s = 1;

                do
                {
                    var res = new TimeSpan(simtype.Interval.Ticks / s);
                    newSimTime = simDataPrev.SimTime + res;
                    s *= 2;
                }
                while (db.SimDatas.Where(x => x.SimTime == newSimTime).Any());
                simdata = new SimData
                {
                    SimTypeID = simtype.SimTypeID,
                    SimTime = newSimTime,
                    Consumption = simDataPrev.Consumption,
                    Wind = simDataPrev.Wind,
                    Sun = simDataPrev.Sun
                };
            }
            else
            {
                simdata = new SimData
                {
                    SimTypeID = simtype.SimTypeID,
                    SimTime = newSimTime
                };
            }
            

            ViewBag.SimTypeID = id;
            ViewData["ReturnTo"] = "../SimTypes/Details/" + id.ToString();
            return View(simdata);
        }

        // POST: SimDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SimDataID,SimTypeID,SimTime,Wind,Sun,Consumption, RealTime")] SimData simData, string ReturnTo, int? id)
        {


            SimType simtype = await db.SimTypes.FindAsync(simData.SimTypeID);

            if (simtype == null)
            {
                throw new NullReferenceException();
            }

            //if (db.SimDatas.Where(x => x.SimTime == simtype.StartTime).Any())
            //{

            //}

            if (simData.SimTime < simtype.StartTime)
            {
                ModelState.AddModelError("SimTime", "The Time has to be greater then " +  simtype.StartTime.ToString() + "!");
            }
            else if (simData.SimTime > simtype.EndTime)
            {
                ModelState.AddModelError("SimTime", "The Time has to be smaller then " + simtype.EndTime.ToString() + "!");
            }
            if (ModelState.IsValid)
            {
                db.SimDatas.Add(simData);
                await db.SaveChangesAsync();
                ViewBag.SimTypeID = simData.SimTypeID;
                return RedirectToAction("Details", "SimTypes", new { id });
            }
            return View(simData);
        }

        // GET: SimDatas/Edit/5
        public async Task<ActionResult> Edit(int? id, string ReturnTo = "Index")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimData simData = await db.SimDatas.FindAsync(id);
            if (simData == null)
            {
                return HttpNotFound();
            }
            ViewData["ReturnTo"] = "../SimTypes/Details/" + simData.SimTypeID.ToString();
            ViewBag.SimTypeID = new SelectList(db.SimTypes, "SimTypeID", "Title", simData.SimTypeID);
            return View(simData);
        }

        // POST: SimDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SimDataID,SimTypeID,SimTime,Wind,Sun,Consumption")] SimData simData, string ReturnTo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(simData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(ReturnTo);
            }
            ViewBag.SimTypeID = new SelectList(db.SimTypes, "SimTypeID", "Title", simData.SimTypeID);
            return View(simData);
        }

        // GET: SimDatas/Delete/5
        public async Task<ActionResult> Delete(int? id, string ReturnTo = "Index")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimData simData = await db.SimDatas.FindAsync(id);
            if (simData == null)
            {
                return HttpNotFound();
            }
            ViewData["ReturnTo"] = "../SimTypes/Details/" + simData.SimTypeID.ToString();
            return View(simData);
        }

        // POST: SimDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, string ReturnTo)
        {
            SimData simData = await db.SimDatas.FindAsync(id);
            int sid = simData.SimTypeID;
            db.SimDatas.Remove(simData);
            await db.SaveChangesAsync();
            return RedirectToAction(ReturnTo);
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
