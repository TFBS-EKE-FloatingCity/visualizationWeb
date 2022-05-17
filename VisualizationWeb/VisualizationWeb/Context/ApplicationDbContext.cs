﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Simulation.Library.Models;
using VisualizationWeb.Models;

namespace VisualizationWeb.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SimScenario> SimScenarios { get; set; }
        public virtual DbSet<SimPosition> SimPositions { get; set; }
        public virtual DbSet<CityData> CityDatas { get; set; }
        public virtual DbSet<CityDataHead> CityDataHeads { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}