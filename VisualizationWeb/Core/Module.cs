namespace Core
{
   public class Module
   {
      public static class Sectors
      {
         public const string One = "One";
         public const string Two = "Two";
         public const string Three = "Three";
      }

      public string sector { get; set; }
      public int sensorOutside { get; set; }
      public int sensorInside { get; set; }
      public int pumpLevel { get; set; }
   }
}