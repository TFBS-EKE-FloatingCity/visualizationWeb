using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Exceptions
{
   public class InvalidScenarioException : Exception
   {
      public InvalidScenarioException() 
         : base("Scenario is invalid. The Scenario has to have at least 2 positions and must last at least 1 second.") { }
   }
}
