using System;

namespace Simulation.Library.Models
{
    public interface ISimulationService : IDisposable
    {
        #region Properties
        /// <summary>
        /// The Id of the SimulationScenario.
        /// </summary>
        int SimulationScenarioId { get; }
        /// <summary>
        /// Tells the definied maximum energyproduction of the windturbines.
        /// </summary>
        int MaxEnergyProductionWind { get; }
        /// <summary>
        /// Tells the definied maximum energyproduction of the suncollectors.
        /// </summary>
        int MaxEnergyProductionSun { get; }
        /// <summary>
        /// Tells the definied maximum energyconsumption of the city.
        /// </summary>
        int MaxEnergyConsumption { get; }
        /// <summary>
        /// Tells if the current is running.
        /// </summary>
        bool IsSimulationRunning { get; }
        /// <summary>
        /// Returns the actual StartedDateTime. Null if the Simulation is not running.
        /// </summary>
        DateTime? StartDateTimeReal { get; }
        /// <summary>
        /// The Factor at which the Simulation runs compared with the real time.
        /// </summary>
        decimal TimeFactor { get; }

        event EventHandler SimulationStarted;
        event EventHandler SimulationEnded;
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the current Time in the Simulation for the given timeStamp.
        /// </summary>
        /// <returns>Returns the current time in the simulation. Null if the simulation is not running.</returns>
        DateTime? GetSimulatedTimeStamp(DateTime timeStamp);
        /// <summary>
        /// Calculates the percental simulated energyproduction for the given timeStamp for the wind turbines.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        int? GetEnergyProductionWind(DateTime timeStamp);
        /// <summary>
        /// Calculates the percental simulated energyproduction for the given timeStamp for the suncollectors.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        int? GetEnergyProductionSun(DateTime timeStamp);
        /// <summary>
        /// Calculates the percental simulated energyconsumption of the city for the given timeStamp.
        /// </summary>
        /// <param name="timeStamp">The real TimeStamp</param>
        /// <returns>The percental simulated energyproduction. Null if the simulation is not running.</returns>
        int? GetEnergyConsumption(DateTime timeStamp);
        /// <summary>
        /// Calculates the energy balance for the given timeStamp. Negativ if the Consumption is higher than the production of Sun and Wind.
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns>The simulated energy balance. Null if the simulation is not running</returns>
        int? GetEnergyBalance(DateTime timeStamp);
        /// <summary>
        /// Sets the SimulationScenario. Stops the Scenario if there currently is another Scenario running.
        /// </summary>
        void SetSimulationScenario(SimScenario scenario);
        /// <summary>
        /// Runs the simulation.
        /// </summary>
        void Run();
        /// <summary>
        /// Stops the simulation.
        /// </summary>
        void Stop();
        #endregion
    }
}