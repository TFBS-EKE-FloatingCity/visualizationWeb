namespace Simulation.Library.Models.Interfaces
{
   public interface ISimulationServiceSettings
    {
        
        int SunMax { get; set; }
        int WindMax { get; set; }
        int ConsumptionMax { get; set; }
    }
}