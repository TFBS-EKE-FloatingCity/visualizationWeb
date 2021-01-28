
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation.Library.Models;
using Simulation.Library.ViewModels;
using Simulation.Library.ViewModels.SimPositionVM;
using Simulation.Library.ViewModels.SimScenarioVM;
using VisualizationWeb.Models.ViewModel;

namespace VisualizationWeb.Models.IRepo
{
    public interface ISimulationRepository
    {
        Task<IEnumerable<SimPositionIndexViewModel>> GetSimPositionIndex(int simScenarioID);
        Task<IEnumerable<SimPositionBindingViewModel>> GetSimPositionBindingList(int simScenarioID);
        Task<IEnumerable<SimScenarioIndexViewModel>> GetSimScenarioIndex();
        Task<SimScenarioDetailsViewModel> GetSimScenarioDetails(int simScenarioID);
        Task CreateScenario(SimScenarioCreateAndEditViewModel scenario);
        Task CreatePosition(SimPositionCreateAndEditViewModel position);
        Task RemoveScenario(int scenarioID);
        Task RemovePosition(int positionID);
        Task<SimScenario> GetSimScenarioByID(int simScenarioID);
        Task<IList<vmSelectListItem>> SimScenarioSelect();
        string GetSimulationTitle(int simScenarionID);

        Task<IEnumerable<SimPosition>> GetSimPositionsByID(int id);
        Setting GetSimulationSetting();
        Task SaveSetting(Setting setting);

    }
}
