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
    public class APIVDashboardController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: API/Dashboard
        [Route("GetSimulationHistory/{id}")]
        [HttpGet]
        public string GetSimulationHistory(int id)
        {
            return JsonConvert.SerializeObject(db.CityDatas.OrderBy(d => d.Simulationtime).Where(d => d.CityDataHeadID == id).ToList());
        }

        //GET: API/Dashboard
        [Route("GetRecentlyGeneratedEnergy/{id}")]
        [HttpGet]
        public string GetRecentlyGeneratedEnergy(int id)
        {
            return JsonConvert.SerializeObject(db.CityDatas.OrderBy(d => d.Simulationtime).Where(d => d.CityDataHeadID == id).Select(d => new { d.Pump1, d.Pump2, d.Pump3, d.WindCurrent, d.SunCurrent, d.ConsumptionCurrent }).ToList());
        }

        [Route("GetMaxValues")]
        [HttpGet]
        public string GetMaxValues()
        {
            return JsonConvert.SerializeObject(db.Settings.Select(d => new { d.SunMax, d.WindMax, d.ConsumptionMax }).ToList());

        }

    }
}
