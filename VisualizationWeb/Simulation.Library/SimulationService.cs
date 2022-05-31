using Simulation.Library.Calculations;
using Simulation.Library.Models;
using Simulation.Library.Models.Interfaces;
using System;
using System.Linq;
using System.Timers;

namespace Simulation.Library
{
   public class SimulationService : IDisposable, ISimulationService
   {
      private DateTime? _startDateTimeReal;
      private SimPosition _prevPosition;
      private SimPosition _nextPosition;
      private SimScenario _simScenario;
      private int _idleEnergyProductionWind;
      private int _idleEnergyProductionSun;
      private int _idleEnergyConsumption;
      private Timer _timer;

      public TimeSpan Duration { get; private set; }

      public DateTime? StartDateTimeReal
      {
         get
         {
            return _startDateTimeReal;
         }
         private set
         {
            _startDateTimeReal = value;
            if (value != null && Duration != null)
            {
               EndDateTimeReal = value.Value + Duration;
               return;
            }

            EndDateTimeReal = null;
         }
      }

      public DateTime? EndDateTimeReal { get; private set; }

      public int MaxEnergyProductionWind { get; private set; }

      public int MaxEnergyProductionSun { get; private set; }

      public int MaxEnergyConsumption { get; private set; }

      public bool IsSimulationRunning { get; private set; }

      public decimal TimeFactor { get; private set; }

      public int SimulationScenarioId => _simScenario == null ? -1 : _simScenario.SimScenarioID;

      public event EventHandler SimulationStarted;

      public event EventHandler SimulationEnded;

      public SimulationService(ISimulationServiceSettings settings)
      {
         SetIdleValues(0, 0, 0);
         SetSettings(settings);
         TimeFactor = 1;
      }

      public void SetSettings(ISimulationServiceSettings settings)
      {
         if (settings == null)
         {
            MaxEnergyConsumption = 0;
            MaxEnergyProductionSun = 0;
            MaxEnergyProductionWind = 0;
            return;
         }

         MaxEnergyConsumption = settings.ConsumptionMax;
         MaxEnergyProductionSun = settings.SunMax;
         MaxEnergyProductionWind = settings.WindMax;
      }

      public void Run(SimScenario scenario, TimeSpan duration)
      {
         try
         {
            SetupForRunning(scenario, duration);
            StartDateTimeReal = DateTime.Now;
            _timer.Start();
            _prevPosition = _simScenario.SimPositions.OrderBy(p => p.TimeRegistered).First();
            _nextPosition = _simScenario.SimPositions.OrderBy(p => p.TimeRegistered).Skip(1).First();
            IsSimulationRunning = true;
            OnSimulationStarted();
         }
         catch (Exception e)
         {
            throw new Exception($"Could not run the Simulation. {e.Message}", e);
         }
      }

      public void Stop()
      {
         IsSimulationRunning = false;
         StartDateTimeReal = null;
         TimeFactor = 1;
         _simScenario = null;
         _prevPosition = null;
         _nextPosition = null;
         _timer.Stop();
         _timer.Elapsed -= OnSimDurationElapsed;
         OnSimulationEnded();
      }

      /// <summary>
      ///   Stops the Simulation on Dispose.
      /// </summary>
      public void Dispose()
      {
         Stop();
      }

      /// <summary>
      ///   Updates to the given timeStamp and returns the simulatedTimeStamp. Returns Null if the
      ///   requested timeStamp is outside the simulated time.
      /// </summary>
      private DateTime? Update(DateTime timeStamp)
      {
         if (IsSimulationRunning == false || IsTimeStampValid(timeStamp) == false)
         {
            return null;
         }

         DateTime simTimeStamp = GetSimulatedTimeStamp(timeStamp).Value;
         RefreshPositions(simTimeStamp);

         if (_prevPosition is null || _nextPosition is null)  //If either of the positions is null means that the given timeStamp is outside the SimulationTimeRange.
         {                                                    //This shouldn't occur since we check if the given timeStamp is valid.
            throw new Exception("At least one SimulationPosition was null.");
         }

         return simTimeStamp;
      }

      /// <summary>
      ///   Calculates the current Time in the Simulation for the given timeStamp.
      /// </summary>
      /// <returns>
      ///   Returns the current time in the simulation. Null if the simulation is not running.
      /// </returns>
      public DateTime? GetSimulatedTimeStamp(DateTime timeStamp)
      {
         if (StartDateTimeReal == null) return null;

         return new DateTime((long)InterpolationHelper.GetValue(StartDateTimeReal.Value.Ticks,
             _simScenario.StartDate.Value.Ticks,
             EndDateTimeReal.Value.Ticks,
             _simScenario.EndDate.Value.Ticks,
             timeStamp.Ticks));
      }

      /// <summary>
      ///   Calculates the percental simulated energyproduction for the given timeStamp for the wind turbines.
      /// </summary>
      /// <param name="timeStamp"> The real TimeStamp </param>
      /// <returns>
      ///   The percental simulated energyproduction. Returns idle values if no Simulation is running.
      /// </returns>
      public int GetEnergyProductionWind(DateTime timeStamp)
      {
         DateTime? simTimeStamp = Update(timeStamp);
         if (simTimeStamp == null) return _idleEnergyProductionWind;

         return (int)InterpolationHelper.GetValue(
            _prevPosition.TimeRegistered.Ticks,
            _prevPosition.WindValue,
            _nextPosition.TimeRegistered.Ticks,
            _nextPosition.WindValue,
            simTimeStamp.Value.Ticks);
      }

      /// <summary>
      ///   Calculates the percental simulated energyproduction for the given timeStamp for the suncollectors.
      /// </summary>
      /// <param name="timeStamp"> The real TimeStamp </param>
      /// <returns>
      ///   The percental simulated energyproduction. Returns idle values if no Simulation is running.
      /// </returns>
      public int GetEnergyProductionSun(DateTime timeStamp)
      {
         DateTime? simTimeStamp = Update(timeStamp);
         if (simTimeStamp == null) return _idleEnergyProductionSun;

         return (int)InterpolationHelper.GetValue(
            _prevPosition.TimeRegistered.Ticks,
            _prevPosition.SunValue,
            _nextPosition.TimeRegistered.Ticks,
            _nextPosition.SunValue,
            simTimeStamp.Value.Ticks);
      }

      /// <summary>
      ///   Calculates the percental simulated energyconsumption of the city for the given timeStamp.
      /// </summary>
      /// <param name="timeStamp"> The real TimeStamp </param>
      /// <returns>
      ///   The percental simulated energyproduction. Returns idle values if no Simulation is running.
      /// </returns>
      public int GetEnergyConsumption(DateTime timeStamp)
      {
         DateTime? simTimeStamp = Update(timeStamp);
         if (simTimeStamp == null) return _idleEnergyConsumption;

         return (int)InterpolationHelper.GetValue(
            _prevPosition.TimeRegistered.Ticks,
            _prevPosition.EnergyConsumptionValue,
            _nextPosition.TimeRegistered.Ticks,
            _nextPosition.EnergyConsumptionValue,
            simTimeStamp.Value.Ticks);
      }

      /// <summary>
      ///   Checks if a given TimeStamp is valid.
      /// </summary>
      /// <param name="timeStamp"> </param>
      /// <returns>
      ///   True if the simulation is running and the simulated time is between the first and the last position of the Scenario.
      /// </returns>
      protected virtual bool IsTimeStampValid(DateTime timeStamp)
      {
         if (IsSimulationRunning == false) return false;
         DateTime? simTimeStamp = GetSimulatedTimeStamp(timeStamp);
         return simTimeStamp >= _simScenario.StartDate && simTimeStamp <= _simScenario.EndDate;
      }

      protected void RefreshPositions(DateTime simTimeStamp)
      {
         if (simTimeStamp >= _nextPosition.TimeRegistered 
            && simTimeStamp <= _prevPosition.TimeRegistered) return;

         _prevPosition = _simScenario.SimPositions
            .OrderBy(p => p.TimeRegistered)
            .Where(p => p.TimeRegistered <= simTimeStamp)
            .LastOrDefault();

         _nextPosition = _simScenario.SimPositions
            .OrderBy(p => p.TimeRegistered)
            .Where(p => p.TimeRegistered >= simTimeStamp)
            .FirstOrDefault();
      }

      protected virtual void OnSimulationStarted()
      {
         SimulationStarted?.Invoke(this, new EventArgs());
      }

      protected virtual void OnSimulationEnded()
      {
         SimulationEnded?.Invoke(this, new EventArgs());
      }

      /// <summary>
      ///   Calculates the energy balance for the given timeStamp. Negativ if the Consumption is higher than the production of Sun and Wind.
      /// </summary>
      /// <param name="timeStamp"> </param>
      /// <returns>
      ///   The simulated energy balance. Returns idle values if no Simulation is running.
      /// </returns>
      public int GetEnergyBalance(DateTime timeStamp)
      {
         int windValue = GetEnergyProductionWind(timeStamp) * MaxEnergyProductionWind;
         int sunValue = GetEnergyProductionSun(timeStamp) * MaxEnergyProductionSun;
         int consumptionValue = GetEnergyConsumption(timeStamp) * MaxEnergyConsumption;

         int balanceValue = windValue + sunValue - consumptionValue;

         return balanceValue >= 0 
            ? (int)InterpolationHelper.InverseLerp(0, MaxEnergyProductionWind + MaxEnergyProductionSun, balanceValue)
            : (int)InterpolationHelper.InverseLerp(0, MaxEnergyConsumption, balanceValue);
      }

      /// <summary>
      ///   Validates the Scenario and Duration and sets the Properties which are required for the simulation to run.
      /// </summary>
      private void SetupForRunning(SimScenario scenario, TimeSpan duration)
      {
         if (!IsValidScenario(scenario))
         {
            throw new Exception("Scenario is invalid. The Scenario has to have at least 2 positions and must last at least 1 second.");
         }

         if (!IsValidDuration(duration))
         {
            TimeSpan maxTime = new TimeSpan(0, 0, 0, 0, Int32.MaxValue);
            throw new Exception($"Duration is invalid. The Duration has to be a value greater than 0 and less than {maxTime}!");
         }

         if (IsSimulationRunning) Stop();   // Stops the previous Scenario if there is one running.

         _simScenario = scenario;
         Duration = duration;
         TimeFactor = InterpolationHelper.InverseLerp(0, Duration.Ticks, _simScenario.GetDuration().Ticks);
         _timer = new Timer(duration.TotalMilliseconds);
         _timer.Elapsed += OnSimDurationElapsed;
      }

      protected bool IsValidScenario(SimScenario scenario)
      {
         return scenario != null 
            && scenario.SimPositions != null 
            && scenario.SimPositions.Count >= 2 
            && scenario.GetDuration().TotalSeconds > 1;
      }

      protected bool IsValidDuration(TimeSpan duration)
      {
         return duration.Ticks > 0 && duration <= new TimeSpan(0, 0, 0, 0, int.MaxValue);
      }

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
      public void SetIdleValues(int energyConsumption, int energyProductionSun, int energyProductionWind)
      {
         _idleEnergyConsumption = energyConsumption >= 0 
            && energyConsumption <= 100 
            ? energyConsumption 
            : _idleEnergyConsumption;

         _idleEnergyProductionSun = energyProductionSun >= 0 
            && energyProductionSun <= 100 
            ? energyProductionSun 
            : _idleEnergyProductionSun;

         _idleEnergyProductionWind = energyProductionWind >= 0 
            && energyProductionWind <= 100 
            ? energyProductionWind 
            : _idleEnergyProductionWind;
      }

      /// <summary>
      ///   Method which stops the running SimScenario after a set TimeInterval.
      /// </summary>
      protected void OnSimDurationElapsed(object sender, ElapsedEventArgs e)
      {
         Stop();
      }
   }
}