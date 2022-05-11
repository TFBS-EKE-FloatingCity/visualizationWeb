using System.Data;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using VisualizationWeb.Context;
using VisualizationWeb.Helpers;

namespace VisualizationWeb.Controllers
{
    [RoutePrefix("API/Dashboard")]
    public class APIVDashboardController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("GetCurrentCityDataHeadID")]
        [HttpGet]
        public object GetCurrentCityDataHeadID()
        {
            return SingletonHolder.CurrentCityDataHeadID;
        }

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
