using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Models {
    public static class SimulationServiceConfig {
        public static string Config {
            get {
                return @"

                    {
                      ""SimulationData"": {
                        ""Wind"": {
                                        ""Unit"": ""Wh"",
                          ""Maximum"": 250000,
                          ""Minimum"": 0
                        },
                        ""Sun"": {
                                        ""Unit"": ""Wh"",
                          ""Maximum"": 250000,
                          ""Minimum"": 0
                        },
                        ""Consumption"": {
                                        ""Unit"": ""Wh"",
                          ""Maximum"": 500000,
                          ""Minimum"": 0
                        }
                                }
                    }

                ";
            } 
        }
    }
}
