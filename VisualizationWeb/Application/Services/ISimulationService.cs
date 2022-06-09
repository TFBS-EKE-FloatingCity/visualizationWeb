using Core;
using DataAccess.Entities;
using System;

namespace Application.Services
{
   public interface ISimulationService : IDisposable
   {
      event EventHandler SimulationStarted;

      event EventHandler SimulationEnded;

      /// <summary>
      ///   The Id of the SimulationScenario.
      /// </summary>
      int SimulationScenarioId { get; }

      /// <summary>
      ///   Tells the definied maximum energyproduction of the windturbines.
      /// </summary>
      int MaxEnergyProductionWind { get; }

      /// <summary>
      ///   Tells the definied maximum energyproduction of the suncollectors.
      /// </summary>
      int MaxEnergyProductionSun { get; }

      /// <summary>
      ///   Tells the definied maximum energyconsumption of the city.
      /// </summary>
      int MaxEnergyConsumption { get; }

      /// <summary>
      ///   Tells if the current is running.
      /// </summary>
      bool IsSimulationRunning { get; }

      /// <summary>
      ///   The Duration the Simulation actually should take.
      /// </summary>
      TimeSpan Duration { get; }

      /// <summary>
      ///   Returns the actual StartedDateTime. Null if the Simulation is not running.
      /// </summary>
      DateTime? StartDateTimeReal { get; }

      /// <summary>
      ///   Returns the actual EndDateTime. Null if the Simulation is not running.
      /// </summary>
      DateTime? EndDateTimeReal { get; }

      /// <summary>
      ///   The Factor at which the Simulation runs compared with the real time.
      /// </summary>
      decimal TimeFactor { get; }

      /// <summary>
      ///   Sets the maximum values for the Service.
      /// </summary>
      /// <param name="settings"> The containing settings. </param>
      void SetSettings(ISimulationServiceSettings settings);

      /// <summary>
      ///   Calculates the current Time in the Simulation for the given timeStamp.
      /// </summary>
      /// <returns>
      ///   Returns the current time in the simulation. Null if the simulation is not running.
      /// </returns>
      DateTime? GetSimulatedTimeStamp(DateTime timeStamp);

      /// <summary>
      ///   Calculates the percental simulated energyproduction for the given timeStamp for the wind turbines.
      /// </summary>
      /// <param name="timeStamp"> The real TimeStamp </param>
      /// <returns>
      ///   The percental simulated energyproduction. Returns idle values if no Simulation is running.
      /// </returns>
      int GetEnergyProductionWind(DateTime timeStamp);

      /// <summary>
      ///   Calculates the percental simulated energyproduction for the given timeStamp for the suncollectors.
      /// </summary>
      /// <param name="timeStamp"> The real TimeStamp </param>
      /// <returns>
      ///   The percental simulated energyproduction. Returns idle values if no Simulation is running.
      /// </returns>
      int GetEnergyProductionSun(DateTime timeStamp);

      /// <summary>
      ///   Calculates the percental simulated energyconsumption of the city for the given timeStamp.
      /// </summary>
      /// <param name="timeStamp"> The real TimeStamp </param>
      /// <returns>
      ///   The percental simulated energyproduction. Returns idle values if no Simulation is running.
      /// </returns>
      int GetEnergyConsumption(DateTime timeStamp);

      /// <summary>
      ///   Calculates the energy balance for the given timeStamp. Negativ if the Consumption is
      ///   higher than the production of Sun and Wind.
      /// </summary>
      /// <param name="timeStamp"> </param>
      /// <returns>
      ///   The simulated energy balance. Returns idle values if no Simulation is running.
      /// </returns>
      int GetEnergyBalance(DateTime timeStamp);

      /// <summary>
      ///   Runs the simulation. If there is a Scenario running when this Method is called, the
      ///   other Scenario will be stopped.
      /// </summary>
      void Run(SimScenario scenario, TimeSpan duration);

      /// <summary>
      ///   Stops the simulation.
      /// </summary>
      void Stop();

      /// <summary>
      ///   Sets the Idle Values which are sent while no Scenario is running.
      /// </summary>
      /// <param name="energyConsumption">
      ///   The energy consumption of the city in percent. Must be a value between 0 and 100 percent.
      /// </param>
      /// <param name="energyProductionSun">
      ///   The energy production of the suncollectors in percent. Must be a value between 0 and 100 percent.
      /// </param>
      /// <param name="energyProductionWind">
      ///   The energy production of the wind turbines in percent. Must be a value between 0 and 100 percent.
      /// </param>
      void SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind);
   }
}