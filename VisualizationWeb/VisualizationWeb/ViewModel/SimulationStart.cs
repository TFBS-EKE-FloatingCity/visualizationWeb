using System;
using System.Web.Mvc;

namespace UI.ViewModel
{
   public class SimulationStart
   {
      public TimeSpan Duration { get; set; }

      public int SimScenarioID { get; set; }

      public SelectList ScenarioSelectList { get; set; }
   }
}