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
        private ApplicationDbContext db = new ApplicationDbContext();

        public SimData GetSimulationData()
        {
            var result = new List<SimData>();

            var simulation = (from c in db.SimulationHistories
                      where c.Canceled == false &&
                            c.CancelTime == null
                      select c).First();

            var simType = db.SimTypes.Find(simulation.SimulationID);
            var simDatas = db.SimDatas.Where(d => d.SimTypeID == simulation.SimulationID).ToList();

            var timeDiff = DateTime.Now.Subtract(simulation.RealStartTime);

            var firstEntry = simDatas.First();
            var timeDiffSeconds = TimeSpan.FromSeconds(timeDiff.TotalSeconds * simType.SimFactor);
            var currentTime = firstEntry.SimTime + timeDiffSeconds;

            SimData prevData = firstEntry;
            SimData nextData = null;

            foreach (var data in simDatas)
            {
                if(data.SimTime <= currentTime && data.SimTime > prevData.SimTime )
                {
                    prevData = data;
                }
            }


            foreach (var data in simDatas)
            {
                if (data.SimTime >= currentTime && (data.SimTime < nextData.SimTime || nextData == null))
                {
                    nextData = data;
                }
            }

            if(nextData == null)
            {
                nextData = prevData;
            }


            var totalSeconds = (firstEntry.SimTime - nextData.SimTime).TotalSeconds;
            var duration = (prevData.SimTime - nextData.SimTime).TotalSeconds;

            var seconds = timeDiffSeconds.TotalSeconds - totalSeconds;
            var progress = seconds / duration;

            return new SimData()
            {
                Consumption = prevData.Consumption + (prevData.Consumption - nextData.Consumption) * progress,
                Sun = prevData.Sun + (prevData.Sun - nextData.Sun) * progress,
                Wind = prevData.Wind + (prevData.Wind - nextData.Wind) * progress,
            };
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}