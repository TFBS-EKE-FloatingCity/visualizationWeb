﻿using System;
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
        public async Task<ActionResult> Index()
        {
            var simDatas = db.SimDatas.Include(s => s.SimType);
            return View(await simDatas.ToListAsync());
        }

        // GET: SimDatas/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SimData simData = await db.SimDatas.FindAsync(id);
        //    if (simData == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(simData);
        //}

        // GET: SimDatas/Create
        public ActionResult Create()
        {
            ViewBag.SimTypeID = new SelectList(db.SimTypes, "SimTypeID", "Title");
            return View();
        }

        // POST: SimDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SimDataID,SimTypeID,SimTime,Wind,Sun,Consumption")] SimData simData)
        {
            if (ModelState.IsValid)
            {
                db.SimDatas.Add(simData);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SimTypeID = new SelectList(db.SimTypes, "SimTypeID", "Title", simData.SimTypeID);
            return View(simData);
        }

        // GET: SimDatas/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.SimTypeID = new SelectList(db.SimTypes, "SimTypeID", "Title", simData.SimTypeID);
            return View(simData);
        }

        // POST: SimDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SimDataID,SimTypeID,SimTime,Wind,Sun,Consumption")] SimData simData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(simData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SimTypeID = new SelectList(db.SimTypes, "SimTypeID", "Title", simData.SimTypeID);
            return View(simData);
        }

        // GET: SimDatas/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(simData);
        }

        // POST: SimDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SimData simData = await db.SimDatas.FindAsync(id);
            db.SimDatas.Remove(simData);
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
