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

        public SimDataView GetSimulationData()
        {
            var simulation = (from c in db.SimulationHistories
                      where c.Canceled == null
                      select c).First();

            var simType = db.SimTypes.Find(simulation.SimTypeID);
            var simDatas = db.SimDatas.Where(d => d.SimTypeID == simulation.SimTypeID).ToList();

            var timeDiff = DateTime.Now.Subtract(simulation.RealStartTime);

            var firstEntry = simDatas.First();

            foreach (var data in simDatas)
            {
                if (data.SimTime < firstEntry.SimTime)
                {
                    firstEntry = data;
                }
            }

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
                if (data.SimTime >= currentTime && (nextData == null || data.SimTime < nextData.SimTime))
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

            return new SimDataView()
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