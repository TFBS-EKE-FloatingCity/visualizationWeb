using System;

namespace Core.Exceptions
{
   public class InvalidScenarioException : Exception
   {
      public InvalidScenarioException() 
         : base("Scenario is invalid. The Scenario has to have at least 2 positions and must last at least 1 second.") { }

      public InvalidScenarioException(string message) 
         : base(message) { }
   }
}
