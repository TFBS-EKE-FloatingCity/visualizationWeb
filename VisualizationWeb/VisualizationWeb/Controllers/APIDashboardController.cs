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
        [HttpPost]
        public SensorData GetCurrentGeneratorsData()
        {
            return db.SensorDatas.FirstOrDefault(); ;
        }
    }
}