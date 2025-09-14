using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using RVA_Flight.Common.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RVA_Flight.Client.ViewModels
{
    public class FlightViewModel : BaseViewModel
    {
        public ObservableCollection<Flight> Flights { get; set; }
        public Flight NewFlight { get; set; }

        public ObservableCollection<City> Cities { get; set; }
        public ObservableCollection<Airplane> Airplanes { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        public ICommand AddFlightCommand { get; }

        public FlightViewModel()
        {
            Flights = new ObservableCollection<Flight>(
                ClientProxy.Instance.FlightService.LoadFlights() ?? new List<Flight>()
            );

            Cities = new ObservableCollection<City>(
                ClientProxy.Instance.CityService.LoadCities() ?? new List<City>()
            );

            Airplanes = new ObservableCollection<Airplane>(
                ClientProxy.Instance.AirplaneService.LoadAirplanes() ?? new List<Airplane>()
            );

            NewFlight = new Flight
            {
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(1),
                DealayMinutes = 0
            };

            AddFlightCommand = new RelayCommand(AddFlight);
        }

        private void AddFlight(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewFlight?.FlightNumber) ||
                NewFlight.Departure == null ||
                NewFlight.Arrival == null ||
                NewFlight.Airplane == null)
            {
                ErrorMessage = "All fields must be filled.";
                return;
            }

            try
            {
                ErrorMessage = "";

                if (NewFlight.DealayMinutes == 0)
                    NewFlight.State = new OnTimeState();
                else
                    NewFlight.State = new DelayedState();

                ClientProxy.Instance.FlightService.SaveFlight(NewFlight);
                Flights.Add(NewFlight);

                NewFlight = new Flight
                {
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now.AddHours(1),
                    DealayMinutes = 0
                };
                OnPropertyChanged(nameof(NewFlight));
            }
            catch (FaultException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error saving flight: " + ex.Message;
            }
        }
    }
}
