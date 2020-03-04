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

    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Settings
        [Authorize(Roles = "Gast")]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Edit");

            }
            Setting setting = db.Settings.FirstOrDefault();
            if (setting == null)
            {
                setting = new Setting
                {
                    SunActive = true,
                    WindActive = true,
                    ConsumptionActive = true,
                    SunMax = 00.00,
                    WindMax = 00.00,
                    ConsumptionMax = 00.00
                };

                db.Settings.Add(setting);
                db.SaveChanges();
            }

            SettingVM settingVM = new SettingVM
            {
                SettingID = setting.SettingID,
                WindMax = UnitCalc.NumberToPrefix(setting.WindMax),
                SunMax = UnitCalc.NumberToPrefix(setting.SunMax),
                ConsumptionMax = UnitCalc.NumberToPrefix(setting.ConsumptionMax),
                SunActive = setting.SunActive,
                WindActive = setting.WindActive,
                ConsumptionActive = setting.ConsumptionActive,
            };

            return View(settingVM);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            Setting setting = new Setting();
            if (id == null)
            {
                setting = db.Settings.FirstOrDefault();
            }
            else
            {
                setting = db.Settings.Find(id);
            }
            if (setting == null)
            {
                return HttpNotFound();
            }
            SettingVM settingVM = new SettingVM {
                SettingID = setting.SettingID,
                SunActive = setting.SunActive,
                WindActive = setting.WindActive,
                ConsumptionActive = setting.ConsumptionActive,
                SunMax = UnitCalc.NumberToPrefix(setting.SunMax),
                WindMax = UnitCalc.NumberToPrefix(setting.WindMax),
                ConsumptionMax = UnitCalc.NumberToPrefix(setting.ConsumptionMax)
            };
            return View(settingVM);
        }

        // POST: Settings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SettingID,WindMax,SunMax,ConsumptionMax,WindActive,SunActive,ConsumptionActive")] SettingVM settingVM)
        {
            if (ModelState.IsValid)
            {
                Setting setting = db.Settings.Find(settingVM.SettingID);
                setting.WindMax = UnitCalc.PrefixToNumber(settingVM.WindMax);
                setting.SunMax = UnitCalc.PrefixToNumber(settingVM.SunMax);
                setting.ConsumptionMax = UnitCalc.PrefixToNumber(settingVM.ConsumptionMax);
                setting.SunActive = settingVM.SunActive;
                setting.WindActive = settingVM.WindActive;
                setting.ConsumptionActive = settingVM.ConsumptionActive;

                db.Entry(setting).State = EntityState.Modified;
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
