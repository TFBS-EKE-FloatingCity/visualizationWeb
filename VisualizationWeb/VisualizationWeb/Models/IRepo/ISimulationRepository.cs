
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
        /// <summary>
        /// Gets All SimPositions for the given simScenarioID.
        /// Converts SimPosition to SimPositionIndexViewModel.
        /// </summary>
        /// <param name="simScenarioID">The selected SimScenario</param>
        Task<IEnumerable<SimPositionIndexViewModel>> GetSimPositionIndex(int simScenarioID);

        /// <summary>
        /// Gets All SimPositions for the given simScenarioID.
        /// Converts SimPosition to SimPositionBindingViewModel.
        /// </summary>
        /// <param name="simScenarioID">The selected SimScenario</param>
        Task<IEnumerable<SimPositionBindingViewModel>> GetSimPositionBindingList(int simScenarioID);

        /// <summary>
        /// Gets All SimScenarios.
        /// Converts SimScenario to SimScenarioIndexViewModel.
        /// Includes SimPositions.
        /// Converts SimPositions to SimPositionIndexViewModel.
        /// </summary>
        Task<IEnumerable<SimScenarioIndexViewModel>> GetSimScenarioIndex();

        /// <summary>
        /// Gets the SimScenario for the given simScenarioID.
        /// Converts SimScenario to SimScenarioDetailViewModel.
        /// Includes SimPositions.
        /// Converts SimPositions to SimPositionIndexViewModel.
        /// </summary>
        /// <param name="simScenarioID">The selected SimScenario</param>
        Task<SimScenarioDetailsViewModel> GetSimScenarioDetails(int simScenarioID);

        /// <summary>
        /// Creates and adds SimScenario from given scenario.
        /// Converts SimScenarioCreateAndEditViewModel to SimScenario.
        /// </summary>
        /// <param name="scenario">SimScenario to add</param>
        Task CreateScenario(SimScenarioCreateAndEditViewModel scenario);

        /// <summary>
        /// Creates and adds SimPosition from given position.
        /// Converts SimPositionCreateAndEditViewModel to SimPosition.
        /// </summary>
        /// <param name="position">SimPosition to add</param>
        Task CreatePosition(SimPositionCreateAndEditViewModel position);

        /// <summary>
        /// Deletes the SimScenario with the given scenarioID.
        /// </summary>
        /// <param name="scenarioID">SimScenarioID to delete</param>
        Task RemoveScenario(int scenarioID);

        /// <summary>
        /// Deletes the SimPosition with the given positionID.
        /// </summary>
        /// <param name="positionID">SimPositionID to delete</param>
        Task RemovePosition(int positionID);

        /// <summary>
        /// FGets SimScenario for the given simScenarioID
        /// </summary>
        /// <param name="simScenarioID">The selected SimScenario</param>
        Task<SimScenario> GetSimScenarioByID(int simScenarioID);

        /// <summary>
        /// Gets all SimScenarios.
        /// Converts SimScenario to vmSelectListItem
        /// </summary>
        Task<IList<vmSelectListItem>> SimScenarioSelect();

        /// <summary>
        /// Gets the Title from the given simScenarioID.
        /// </summary>
        /// <param name="simScenarionID">The selected SimScenario</param>
        string GetSimulationTitle(int simScenarionID);

        /// <summary>
        /// Gets all SimPositions for the given simScenarioID.
        /// </summary>
        /// <param name="id">The selected SimScenario</param>
        Task<IEnumerable<SimPosition>> GetSimPositionsByID(int id);

        /// <summary>
        /// Gets Simulation Setting
        /// </summary>
        Setting GetSimulationSetting();

        /// <summary>
        /// Saves and adds the bgiven setting
        /// </summary>
        /// <param name="setting">Setting to add</param>
        Task SaveSetting(Setting setting);

    }
}
