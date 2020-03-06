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
    public class SimTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public SimTypesController()
        {
            ViewBag.ActiveNav = "simulation";
        }

        // GET: SimTypes
        public async Task<ActionResult> Index()
        {
            var SimTypes = from s in db.SimTypes
                           orderby s.SimDatas, s.SimFactor
                           select s;

            return View(await db.SimTypes.ToListAsync());
        }

        // GET: SimTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimType simType = await db.SimTypes.FindAsync(id);
            if (simType == null)
            {
                return HttpNotFound();
            }

            ViewBag.showGenerateButton = !db.SimDatas.Where(i => i.SimTypeID == simType.SimTypeID).Any();

            return View(simType);
        }

        // GET: SimTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SimTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SimTypeID,Title,SimFactor,StartTime,Interval,EndTime,Notes")] SimType simType)
        {
            if (ModelState.IsValid)
            {
                db.SimTypes.Add(simType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(simType);
        }

        // GET: SimDatas/Edit/5
        public async Task<ActionResult> EditHead(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimType simType = await db.SimTypes.FindAsync(id);
            if (simType == null)
            {
                return HttpNotFound();
            }
            ViewData["ReturnTo"] = "Details/" + id.ToString();
            return View("../SimTypes/Edit", simType);
        }

        // POST: SimDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditHead([Bind(Include = "SimTypeID,Title,SimFactor,StartTime,Interval,EndTime,Notes")] SimType simType, string ReturnTo)
        {
            if (ModelState.IsValid)
            {
                if (simType.StartTime < simType.EndTime)
                {
                    db.Entry(simType).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction(ReturnTo.ToString());
                }
                else
                {
                    // TODO
                }
                
            }
            return View(simType);
        }

        // GET: SimTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimType simType = await db.SimTypes.FindAsync(id);
            if (simType == null)
            {
                return HttpNotFound();
            }


            var t = simType.EndTime - simType.StartTime;
            int x = Convert.ToInt16(Math.Ceiling(t.TotalMinutes / simType.Interval.TotalMinutes));
            Random random = new Random();

            var simtime = simType.StartTime;

            if (!db.SimDatas.Where(i => i.SimTypeID == simType.SimTypeID).Any())
            {
                for (int i = 0; i < x + 1; i++)
                {
                    db.SimDatas.Add(new SimData()
                    {
                        SimTime = simtime,
                        SimTypeID = simType.SimTypeID,
                        Wind = random.Next(100),
                        Sun = random.Next(0, 100),
                        Consumption = random.Next(10, 80),
                    });

                    simtime += simType.Interval;
                }
                await db.SaveChangesAsync();   
            }
            

            return RedirectToAction("Details", new { id  });
        }

        // GET: SimTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimType simType = await db.SimTypes.FindAsync(id);
            if (simType == null)
            {
                return HttpNotFound();
            }
            return View(simType);
        }

        // POST: SimTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SimType simType = await db.SimTypes.FindAsync(id);
            db.SimTypes.Remove(simType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //TODO

        // GET: SimTypes/Delete/5
        public async Task<ActionResult> DeletePos(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimType simType = await db.SimTypes.FindAsync(id);
            if (simType == null)
            {
                return HttpNotFound();
            }

            var simDatas = simType.SimDatas.Where(x => x.SimTypeID == id);

            if (simDatas == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.SimDatas.RemoveRange(simDatas);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }


        // POST: SimTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedPos(int id)
        {
            SimType simType = await db.SimTypes.FindAsync(id);
            db.SimTypes.Remove(simType);
            await db.SaveChangesAsync();
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
