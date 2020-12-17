using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisualizationWeb.Models;

namespace VisualizationWeb.Models.Repo
{
    public class SimulationRepository : ISimulationRepository
    {
        private readonly ApplicationDbContext _context;

        public SimulationRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
    }
}