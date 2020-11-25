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
        [Route("GetCurrentGeneratorsData")]
        [HttpGet]
        public string GetCurrentGeneratorsData()
        {
            return JsonConvert.SerializeObject(db.SensorDatas.OrderByDescending(d => d.MeasureTime).Where(s => (s.SensorID == 10) || (s.SensorID == 11) || (s.SensorID == 12)).Take(3).ToList());
        }
        
        // GET: API/Dashboard
        [Route("GetCurrentPumpsData")]
        [HttpGet]
        public string GetCurrentPumpsData()
        {
            return JsonConvert.SerializeObject(db.SensorDatas.OrderByDescending(d => d.MeasureTime).Where(s => (s.SensorID == 7) || (s.SensorID == 8) || (s.SensorID == 9)).Take(3).ToList());
        }
    }
}