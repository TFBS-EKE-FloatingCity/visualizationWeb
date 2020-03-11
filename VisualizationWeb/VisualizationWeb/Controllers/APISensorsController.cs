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
    [RoutePrefix("API/Sensors")]
    public class APISensorsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/APISensors
        [Route("")]
        [HttpPost]
        public string GetSensors()
        {
            var json = Request.Content.ReadAsStringAsync().Result;
            var account = JsonConvert.DeserializeObject<List<SensorDataApi>>(json);
            
            JsonSerializer jsonSerializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetTempPath(), "Test.json")))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                jsonSerializer.Serialize(writer, account);
                
            }

            return "";
            //foreach (var item in account)
            //{
            //    switch (item.Sensor)
            //    {
            //        case 1:
            //            //do some magic
            //            break;
            //        case 2:
            //            //do some magic
            //            break;
            //        case 3:
            //            //do some magic
            //            break;
            //        case 4:
            //            //do some magic
            //            break;
            //        case 5:
            //            //do some magic
            //        case 6:
            //            //do some magic
            //            break;
            //        case 7:
            //            //do some magic
            //            break;
            //        case 8:
            //            //do some magic
            //            break;
            //        case 9:
            //            //do some magic
            //            break;
            //        case 10:
            //            //do some magic
            //            break;
            //        case 11:
            //            //do some magic
            //            break;
            //        case 12:
            //            //do some magic
            //            break;
            //        case 13:
            //            //do some magic
            //            break;
            //        case 14:
            //            //do some magic
            //            break;
            //        case 15:
            //            //do some magic
            //            break;
            //        default:
            //            throw new NotImplementedException();
            //    }
            //}
            //return "";



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