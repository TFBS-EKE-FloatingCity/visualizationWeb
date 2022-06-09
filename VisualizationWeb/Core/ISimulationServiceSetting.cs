namespace Core
{
   public interface ISimulationServiceSettings
   {
      int SunMax { get; set; }
      int WindMax { get; set; }
      int ConsumptionMax { get; set; }
   }
}