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

namespace VisualizationWeb.Controllers
{
    [RoutePrefix("API/Sensors")]
    public class APISensorsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/APISensors
        [Route("")]
        [HttpPost]
        public List<SimData> GetSensors()
        {
            var json = Request.Content.ReadAsStringAsync().Result;
            var sensorData = JsonConvert.DeserializeObject<List<SensorDataApi>>(json);

            // Process input data
            foreach(var item in sensorData)
            {
                string Value = Convert.ToString(item.Value0 + item.Value1, 2);
                int finalValue = Convert.ToInt32(Value, 2);
                // TODO: add magic convertion here

                var data = new SensorData
                {
                    SensorID = item.Sensor,
                    MeasureTime = new DateTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).Ticks + item.Timestamp * TimeSpan.TicksPerSecond),
                    SValue = finalValue
                };
            }

            var simulationHelper = new SimulationHelper();
            var result = simulationHelper.GetSimulationData();
            simulationHelper.Dispose();

            return result;
        }

        // GET: api/APISensors/5
        [ResponseType(typeof(Sensor))]
        public async Task<IHttpActionResult> GetSensor(int id)
        {
            Sensor sensor = await db.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // PUT: api/APISensors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSensor(int id, Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensor.SensorID)
            {
                return BadRequest();
            }

            db.Entry(sensor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/APISensors
        [ResponseType(typeof(Sensor))]
        public async Task<IHttpActionResult> PostSensor(Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sensors.Add(sensor);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sensor.SensorID }, sensor);
        }

        // DELETE: api/APISensors/5
        [ResponseType(typeof(Sensor))]
        public async Task<IHttpActionResult> DeleteSensor(int id)
        {
            Sensor sensor = await db.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }

            db.Sensors.Remove(sensor);
            await db.SaveChangesAsync();

            return Ok(sensor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SensorExists(int id)
        {
            return db.Sensors.Count(e => e.SensorID == id) > 0;
        }
    }
}