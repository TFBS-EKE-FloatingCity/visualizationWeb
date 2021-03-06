using Application;
using DataAccess;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace UI.Controllers
{
   [RoutePrefix("API/Dashboard")]
   public class APIVDashboardController : ApiController
   {
      private Context _db = new Context();

      [Route("GetCurrentCityDataHeadID")]
      [HttpGet]
      public object GetCurrentCityDataHeadID()
      {
         return Mediator.CurrentCityDataHeadID;
      }

      // GET: API/Dashboard
      [Route("GetSimulationHistory/{id}")]
      [HttpGet]
      public string GetSimulationHistory(int id)
      {
         return JsonConvert.SerializeObject(_db.CityDatas.OrderBy(d => d.Simulationtime)
            .Where(d => d.CityDataHeadID == id).ToList());
      }

      //GET: API/Dashboard
      [Route("GetRecentlyGeneratedEnergy/{id}")]
      [HttpGet]
      public string GetRecentlyGeneratedEnergy(int id)
      {
         return JsonConvert.SerializeObject(_db.CityDatas.OrderBy(d => d.Simulationtime)
            .Where(d => d.CityDataHeadID == id)
            .Select(d => new { d.Pump1, d.Pump2, d.Pump3, d.WindCurrent, d.SunCurrent, d.ConsumptionCurrent })
            .ToList());
      }

      [Route("GetMaxValues")]
      [HttpGet]
      public string GetMaxValues()
      {
         return JsonConvert.SerializeObject(
            _db.Settings.Select(d => new { d.SunMax, d.WindMax, d.ConsumptionMax }
            ).ToList()
         );
      }
   }
}