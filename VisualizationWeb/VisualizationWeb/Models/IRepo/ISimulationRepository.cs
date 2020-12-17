using Simulation.Library.Models.ViewModels.SimPositionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation.Library.Models.ViewModels.SimScenarioVM;




namespace VisualizationWeb.Models.IRepo
{
    public interface ISimulationRepository
    {
        Task<IEnumerable<SimPositionIndexViewModel>> GetSimPositionIndex();
        Task<IEnumerable<SimScenarioIndexViewModel>> GetSimScenarioIndex();
        Task<IEnumerable<SimScenarioDetailsViewModel>> GetSimScenarioDetails(int simScenarioID);
        Task CreateScenario(SimScenarioCreateAndEditViewModel scenario);
        Task CreatePosition(SimPositionCreateAndEditViewModel position);
        Task RemoveScenario(int scenarioID);
        Task RemovePosition(int positionID);
    }
}
