using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisualizationWeb.Models;

namespace VisualizationWeb.Helpers
{
    public class SimulationHelper : IDisposable
    {

        public List<SimData> GetSimulationData()
        {
            var result = new List<SimData>();

            // TODO: Load the data from the database!!
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                result.Add(new SimData()
                {
                    Consumption = random.Next(0, 100),
                    Sun = random.Next(0, 100),
                    Wind = random.Next(0, 100),
                    SimTime = DateTime.Now,
                    SimTypeID = 1
                });
            }

            return result;
        }

        public void Dispose()
        {
         
        }
    }
}