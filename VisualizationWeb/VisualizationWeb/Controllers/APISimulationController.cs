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
using VisualizationWeb.Helpers;
using VisualizationWeb.Models;
using VisualizationWeb.Models.ViewModel;

namespace VisualizationWeb.Controllers
{
    [RoutePrefix("API/Simulation")]
    public class APISimulationController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Simulation
        [Route("GetSimulationTimes")]
        public string GetSimulationTimes()
        {
            SimulationTimesVM simulationTimesVM = new SimulationTimesVM
            {
                RealStartTime = DateTime.Now,
                StartTime = new DateTime(2020, 03, 05, 07, 0, 0),
                EndTime = new DateTime(2020, 03, 05, 12, 0, 0),
                SimFactor = 10
            };

            return JsonConvert.SerializeObject(simulationTimesVM);
        }

        // GET: api/Simulation
        [Route("CheckSimulationStatus")]
        public bool CheckSimulationStatus()
        {
            return true;
        }
    }
}