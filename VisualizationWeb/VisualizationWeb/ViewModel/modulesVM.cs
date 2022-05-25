namespace VisualizationWeb.ViewModel
{
   public class modulesVM
   {
      public const string SectorOne = "One";
      public const string SectorTwo = "Two";
      public const string SectorThree = "Three";

      public string sector { get; set; }
      public int sensorOutside { get; set; }
      public int sensorInside { get; set; }
      public int pumpLevel { get; set; }
   }
}