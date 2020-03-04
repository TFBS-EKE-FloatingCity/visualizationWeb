using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisualizationWeb.Helpers;
using VisualizationWeb.Models;
using VisualizationWeb.Models.ViewModel;

namespace VisualizationWeb.Controllers
{
    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SettingVM settingVM = new SettingVM();

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

            settingVM.SettingID = setting.SettingID;
            settingVM.WindMax = UnitCalc.NumberToPrefix(setting.WindMax);
            settingVM.SunMax = UnitCalc.NumberToPrefix(setting.SunMax);
            settingVM.ConsumptionMax = UnitCalc.NumberToPrefix(setting.ConsumptionMax);
            settingVM.SunActive = setting.SunActive;
            settingVM.WindActive = setting.WindActive;
            settingVM.ConsumptionActive = setting.ConsumptionActive;

            UnitCalc.PrefixToNumber("10 GW");

            return View(settingVM);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(settingVM);
        }

        // POST: Settings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SettingID,WindMax,SunMax,ConsumptionMax,WindActive,SunActive,ConsumptionActive")] SettingVM settingVM)
        {
            if (ModelState.IsValid)
            {
                Setting set = db.Settings.Find(settingVM.SettingID);
                set.WindMax = UnitCalc.PrefixToNumber(settingVM.WindMax);
                set.SunMax = UnitCalc.PrefixToNumber(settingVM.SunMax);
                set.ConsumptionMax = UnitCalc.PrefixToNumber(settingVM.ConsumptionMax);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(settingVM);
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
