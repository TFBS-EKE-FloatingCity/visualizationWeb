using System;

namespace Simulation.Library.Models
{
    public interface ISimulationService : IDisposable
    {

        event EventHandler SimulationStarted;
        event EventHandler SimulationEnded;

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
        /// Returns the definied maximum energyproduction of the windturbines.
        /// </summary>
        int GetMaxEnergyProductionWind();
        /// <summary>
        /// Returns the definied maximum energyproduction of the suncollectors.
        /// </summary>
        int GetMaxEnergyProductionSun();
        /// <summary>
        /// Returns the definied maximum energyconsumption of the city.
        /// </summary>
        int GetMaxEnergyConsumption();
        /// <summary>
        /// Returns if the current is running.
        /// </summary>
        bool IsSimulationRunning();
        /// <summary>
        /// Runs the simulation.
        /// </summary>
        void Run();
        /// <summary>
        /// Stops the simulation.
        /// </summary>
        void Stop();
    }
}