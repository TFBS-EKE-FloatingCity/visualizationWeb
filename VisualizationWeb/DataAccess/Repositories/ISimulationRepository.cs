using DataAccess.Entities;
using DataAccess.Entities.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
   public interface ISimulationRepository
   {
      /// <summary>
      ///   Gets All SimPositions for the given simScenarioID. Converts SimPosition to SimPositionIndexViewModel.
      /// </summary>
      /// <param name="simScenarioID"> The selected SimScenario </param>
      Task<IEnumerable<SimPositionIndex>> GetSimPositionIndex(int simScenarioID);

      /// <summary>
      ///   Gets All SimPositions for the given simScenarioID. Converts SimPosition to SimPositionBindingViewModel.
      /// </summary>
      /// <param name="simScenarioID"> The selected SimScenario </param>
      Task<IEnumerable<SimPositionBinding>> GetSimPositionBindingList(int simScenarioID);

      /// <summary>
      ///   Gets All SimScenarios. Converts SimScenario to SimScenarioIndexViewModel. Includes
      ///   SimPositions. Converts SimPositions to SimPositionIndexViewModel.
      /// </summary>
      Task<IEnumerable<SimScenarioIndex>> GetAllSimScenarioIndices();

      /// <summary>
      ///   Gets the SimScenario for the given simScenarioID. Converts SimScenario to
      ///   SimScenarioDetailViewModel. Includes SimPositions. Converts SimPositions to SimPositionIndexViewModel.
      /// </summary>
      /// <param name="simScenarioID"> The selected SimScenario </param>
      Task<SimScenarioDetails> GetSimScenarioDetails(int simScenarioID);

      /// <summary>
      ///   Creates and adds SimScenario from given scenario. Converts
      ///   SimScenarioCreateAndEditViewModel to SimScenario.
      /// </summary>
      /// <param name="scenario"> SimScenario to add </param>
      Task CreateScenario(SimScenarioCreateAndEdit scenario);

      /// <summary>
      ///   Creates and adds SimPosition from given position. Converts
      ///   SimPositionCreateAndEditViewModel to SimPosition.
      /// </summary>
      /// <param name="position"> SimPosition to add </param>
      Task CreatePosition(SimPositionCreateAndEdit position);

      /// <summary>
      ///   Deletes the SimScenario with the given scenarioID.
      /// </summary>
      /// <param name="scenarioID"> SimScenarioID to delete </param>
      Task DeleteScenario(int scenarioID);

      /// <summary>
      ///   Deletes the SimPosition with the given positionID.
      /// </summary>
      /// <param name="positionID"> SimPositionID to delete </param>
      Task DeletePosition(int positionID);

      /// <summary>
      ///   FGets SimScenario for the given simScenarioID
      /// </summary>
      /// <param name="simScenarioID"> The selected SimScenario </param>
      Task<SimScenario> GetSimScenarioByID(int simScenarioID);

      /// <summary>
      ///   Gets all SimScenarios. Converts SimScenario to vmSelectListItem
      /// </summary>
      Task<IList<SelectListItem>> GetSimScenarioSelectList();

      /// <summary>
      ///   Gets the Title from the given simScenarioID.
      /// </summary>
      /// <param name="simScenarionID"> The selected SimScenario </param>
      string GetSimulationTitle(int simScenarionID);

      /// <summary>
      ///   Gets all SimPositions for the given simScenarioID.
      /// </summary>
      /// <param name="id"> The selected SimScenario </param>
      Task<IEnumerable<SimPosition>> GetSimPositionsByID(int id);

      /// <summary>
      ///   Gets Simulation Setting
      /// </summary>
      Setting GetSimulationSetting();

      /// <summary>
      ///   Saves and adds the bgiven setting
      /// </summary>
      /// <param name="setting"> Setting to add </param>
      /// <returns>true if connectionstring changed, false if not</returns>
      Task<bool> SaveSetting(Setting setting);
   }
}