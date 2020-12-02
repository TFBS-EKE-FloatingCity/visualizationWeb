using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VisualizationWeb.Models;

namespace VisualizationWeb.Controllers
{
    [RoutePrefix("API/Dashboard")]
    public class APIDashboardController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: API/Dashboard
        //Get Last 3 Generatorentries from the SensorData-Table
        //Available Columns: SensorDataID, SensorID, SValue, MeasureTime
        [Route("GetCurrentGeneratorsData")]
        [HttpGet]
        public string GetCurrentGeneratorsData()
        {
            return JsonConvert.SerializeObject(db.SensorDatas.OrderByDescending(d => d.MeasureTime).Where(s => (s.SensorID == 10) || (s.SensorID == 11) || (s.SensorID == 12)).Take(3).ToList());
        }

        // GET: API/Dashboard
        //Get Last 3 Pumpentries of the SensorData-Table
        //Available Columns: SensorDataID, SensorID, SValue, MeasureTime
        [Route("GetCurrentPumpsData")]
        [HttpGet]
        public string GetCurrentPumpsData()
        {
            return JsonConvert.SerializeObject(db.SensorDatas.OrderByDescending(d => d.MeasureTime).Where(s => (s.SensorID == 7) || (s.SensorID == 8) || (s.SensorID == 9)).Take(3).ToList());
        }


        //Get: API/Dashboard
        //Get MaxValues and ifActive from the Settings Table
        //Available Columns: SettingID, WindMax, SunMax, ConsumptionMax, WindActive, SunActive, ConsumptionActive
        [Route("GetCurrentSettings")]
        [HttpGet]
        public string GetCurrentSettings()
        {
            return JsonConvert.SerializeObject(db.Settings.OrderByDescending(s => s.SettingID).Take(1).ToList());
        }

        //Get: API/Dashboard
        //Get current Simulationdata
        //Available Columns: SimDataID, SimTypeID, SimTime, Wind, Sund, Consumption
        [Route("GetCurrentSimDatas")]
        [HttpGet]
        public string GetCurrentSimDatas()
        {
            return JsonConvert.SerializeObject(db.SimDatas.OrderByDescending(sd => sd.SimTime).Take(1).ToList());
        }


    }
}