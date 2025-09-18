using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using RVA_Flight.Common.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace RVA_Flight.Client.ViewModels
{
    public class FlightStateSimulationViewModel : BaseViewModel
    {

        public ObservableCollection<Flight> SimulatedFlights { get; set; }

        public ICommand StartSimulationCommand { get; }
        public ICommand StopSimulationCommand { get; }


        private DispatcherTimer _timer;
        private Random _random;


        public FlightStateSimulationViewModel()
        {

            SimulatedFlights = new ObservableCollection<Flight>(
               ClientProxy.Instance.FlightService.LoadFlights() ?? new List<Flight>()
           );
            _random = new Random();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2); // svaka 2 sekunde
            _timer.Tick += (s, e) => StartSimulation(null);

            StartSimulationCommand = new RelayCommand(obj =>
            {
                if (!_timer.IsEnabled)
                    _timer.Start();
            });

            StopSimulationCommand = new RelayCommand(obj =>
            {
                if (_timer.IsEnabled)
                    _timer.Stop();
            });

            //StartSimulationCommand = new RelayCommand(StartSimulation);
            //StopSimulationCommand = new RelayCommand(StopSimulation);

        }

       


        private void StartSimulation(object obj)
        {
            foreach (var flight in SimulatedFlights)
            {
                int stateIndex = _random.Next(0, 3); // 0 = OnTime, 1 = Delayed, 2 = Cancelled

                switch (stateIndex)
                {
                    case 0: // OnTime
                        flight.State = new OnTimeState();
                        flight.DelayMinutes = 0;
                        break;
                    case 1: // Delayed
                        flight.State = new DelayedState();
                        flight.DelayMinutes = _random.Next(5, 121); // random delay 5-120 min
                        break;
                    case 2: // Cancelled
                        flight.State = new CancelledState();
                        flight.DelayMinutes = 0;
                        break;
                }

                flight.State.SetFlight(flight);
                flight.PilotMessage = flight.State.GetPilotMessage();
            }
            SimulatedFlights = new ObservableCollection<Flight>(SimulatedFlights);
            OnPropertyChanged(nameof(SimulatedFlights));
        }

        private void StopSimulation(object obj)
        {
            
        }

       


    }
}
