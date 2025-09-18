using RVA_Flight.Client.Commands;
using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using RVA_Flight.Common.Enums;
using RVA_Flight.Common.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using log4net;

namespace RVA_Flight.Client.ViewModels
{
    public class FlightViewModel : BaseViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FlightViewModel));
        public Commands.CommandManager CommandManager { get; }

        public ObservableCollection<Flight> Flights { get; set; }
        public ObservableCollection<Flight> FilteredFlights { get; set; }

        public ObservableCollection<Flight> DisplayWithCharters { get; set; }


        public ObservableCollection<CharterFlight> CharterFlights { get; set; }



        public Flight NewFlight { get; set; }

        private Flight _updatedFlight;
        public Flight UpdatedFlight
        {
            get => _updatedFlight;
            set
            {
                if (_updatedFlight == value) return;
                _updatedFlight = value;
                OnPropertyChanged(nameof(UpdatedFlight));
            }
        }

        private Flight _selectedFlight;

        public Flight SelectedFlight
        {
            get => _selectedFlight;
            set
            {
                if (_selectedFlight == value) return;
                _selectedFlight = value;
                OnPropertyChanged(nameof(SelectedFlight));
                PrepareUpdatedFlight();
                log.Info($"Selected flight for update: {SelectedFlight?.FlightNumber}");
            }
        }

        private Flight _selectedFlightForDelete;
        public Flight SelectedFlightForDelete
        {
            get => _selectedFlightForDelete;
            set { _selectedFlightForDelete = value; OnPropertyChanged(nameof(SelectedFlightForDelete)); }
        }


        private string _selectedSearchProperty;
        public string SelectedSearchProperty
        {
            get => _selectedSearchProperty;
            set { _selectedSearchProperty = value; OnPropertyChanged(nameof(SelectedSearchProperty)); }

        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
        }

        public ObservableCollection<City> Cities { get; set; }
        public ObservableCollection<Airplane> Airplanes { get; set; }

        private string _addErrorMessage;
        public string AddErrorMessage
        {
            get => _addErrorMessage;
            set { _addErrorMessage = value; OnPropertyChanged(nameof(AddErrorMessage)); }
        }

        private string _updateErrorMessage;
        public string UpdateErrorMessage
        {
            get => _updateErrorMessage;
            set { _updateErrorMessage = value; OnPropertyChanged(nameof(UpdateErrorMessage)); }
        }

        private string _deleteErrorMessage;
        public string DeleteErrorMessage
        {
            get => _deleteErrorMessage;
            set { _deleteErrorMessage = value; OnPropertyChanged(nameof(DeleteErrorMessage)); }
        }


        public List<string> FlightSearchProperties { get; } =
            new List<string>
            {
                "Flight Number",
                "Departure",
                "Arrival",
                "Airplane",
                "State",
                "Type",
            };
        public ICommand AddFlightCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ShowAllCommand { get; }
        public ICommand UpdateFlightCommand { get; }
        public ICommand DeleteFlightCommand { get; }
        public ICommand UndoOperationCommand { get; }
        public ICommand RedoOperationCommand { get; }
        public ICommand ShowChartersCommand { get; }


        public FlightViewModel()
        {
            log.Info("Initializing FlightViewModel...");

            CommandManager = new Commands.CommandManager();

            Flights = new ObservableCollection<Flight>(
                ClientProxy.Instance.FlightService.LoadFlights() ?? new List<Flight>()
            );
            log.Info($"Loaded {Flights.Count} flights from service.");

            FilteredFlights = new ObservableCollection<Flight>(Flights);

            CharterFlights = new ObservableCollection<CharterFlight>
            {
                new CharterFlight{ FlightNumber="CH100", Type=FlightType.PRIVATE, ArrivalTime=DateTime.Now.AddHours(2), Duration=TimeSpan.FromHours(2)},
                new CharterFlight{ FlightNumber="CH101", Type=FlightType.PRIVATE, ArrivalTime=DateTime.Now.AddHours(1), Duration=TimeSpan.FromHours(3)},
            };

            DisplayWithCharters = new ObservableCollection<Flight>(Flights);
            foreach (var c in CharterFlights)
                DisplayWithCharters.Add(new CharterFlightAdapter(c));





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
                DelayMinutes = 0
            };

            

            AddFlightCommand = new RelayCommand(AddFlight);
            SearchCommand = new RelayCommand(SearchFlights);
            ShowAllCommand = new RelayCommand(ShowAll);
            UpdateFlightCommand = new RelayCommand(UpdateFlight);
            DeleteFlightCommand = new RelayCommand(DeleteFlight, CanDeleteFlight);
            UndoOperationCommand = new RelayCommand(UndoOperation, CanUndoOperation);
            RedoOperationCommand = new RelayCommand(RedoOperation, CanRedoOperation);
            ShowChartersCommand = new RelayCommand(ShowCharterFlights);

            log.Info("FlightViewModel initialized successfully.");
        }

        private void AddFlight(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewFlight?.FlightNumber) ||
                NewFlight.Departure == null ||
                NewFlight.Arrival == null ||
                NewFlight.Airplane == null)
            {
                AddErrorMessage = "All fields must be filled.";
                log.Warn("Attempted to add flight with missing fields.");
                return;
            }

            try
            {
                AddErrorMessage = "";

                if (NewFlight.DelayMinutes == 0)
                    NewFlight.State = new OnTimeState();
                else
                    NewFlight.State = new DelayedState();

                log.Info($"Adding flight {NewFlight.FlightNumber}...");
                AddFlightCommand add = new AddFlightCommand(Flights, NewFlight);
                CommandManager.ExecuteCommand(add);
                log.Info($"Flight {NewFlight.FlightNumber} added successfully.");

                //ponistavanje filtera

                FilteredFlights = new ObservableCollection<Flight>(Flights);
                OnPropertyChanged(nameof(FilteredFlights));

                SearchText = string.Empty;
                SelectedSearchProperty = null;




                NewFlight = new Flight
                {
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now.AddHours(1),
                    DelayMinutes = 0
                };
                OnPropertyChanged(nameof(NewFlight));
            }
            catch (FaultException ex)
            {
                AddErrorMessage = ex.Message;
                log.Warn($"FaultException adding flight: {ex.Message}");
            }
            catch (Exception ex)
            {
                AddErrorMessage = "Error saving flight: " + ex.Message;
                log.Error("Exception adding flight", ex);
            }
        }

        private void PrepareUpdatedFlight()
        {
            if (SelectedFlight == null)
            {
                UpdatedFlight = null;
                return;
            }

           
            UpdatedFlight = new Flight
            {
                FlightNumber = SelectedFlight.FlightNumber,
                Type = SelectedFlight.Type,
                DepartureTime = SelectedFlight.DepartureTime,
                ArrivalTime = SelectedFlight.ArrivalTime,
                PilotMessage = SelectedFlight.PilotMessage,
                DelayMinutes = SelectedFlight.DelayMinutes,
                Airplane = Airplanes.FirstOrDefault(a => a.Name == SelectedFlight.Airplane.Name),
                Departure = Cities.FirstOrDefault(c => c.Name == SelectedFlight.Departure.Name),
                Arrival = Cities.FirstOrDefault(c => c.Name == SelectedFlight.Arrival.Name),
                State = SelectedFlight.State
            };

        }

        private void UpdateFlight(object obj)
        {
            if (SelectedFlight == null || UpdatedFlight == null)
                return; // ništa nije izabrano

            log.Info($"Updating flight {UpdatedFlight.FlightNumber}...");
            // 1. ažuriraj originalni let sa vrednostima iz UpdatedFlight
            UpdateFlightCommand update = new UpdateFlightCommand(Flights, UpdatedFlight);
            CommandManager.ExecuteCommand(update);

            // 2. osveži prikaz DataGrid-a (ako koristiš FilteredFlights)
            FilteredFlights = new ObservableCollection<Flight>(Flights);
            OnPropertyChanged(nameof(FilteredFlights));
            log.Info($"Flight {UpdatedFlight.FlightNumber} updated successfully.");
        }

        private void ShowCharterFlights(object obj)
        {
            DisplayWithCharters = new ObservableCollection<Flight>(Flights);
            foreach (var c in CharterFlights)
                DisplayWithCharters.Add(new CharterFlightAdapter(c));
            log.Info("Showing charter flights...");
            FilteredFlights = new ObservableCollection<Flight>(DisplayWithCharters);
            OnPropertyChanged(nameof(FilteredFlights));

            SearchText = string.Empty;
            SelectedSearchProperty = null;

        }
        private void ShowAll(object obj)
        {
            log.Info("Showing all flights...");
            FilteredFlights = new ObservableCollection<Flight>(Flights);
            OnPropertyChanged(nameof(FilteredFlights));

            SearchText = string.Empty;
            SelectedSearchProperty = null;
            
        }

        private bool CanDeleteFlight(object obj)
        {
            return SelectedFlightForDelete != null; // dugme je aktivno samo ako je nešto izabrano
        }
        private void DeleteFlight(object obj)
        {
            if (SelectedFlightForDelete != null)
            {
                try
                {
                    log.Info($"Deleting flight {SelectedFlightForDelete.FlightNumber}...");
                    DeleteFlightCommand delete = new DeleteFlightCommand(Flights, SelectedFlightForDelete);
                    CommandManager.ExecuteCommand(delete);
                    log.Info($"Flight deleted successfully.");

                    //ponistavanje filtera

                    FilteredFlights = new ObservableCollection<Flight>(Flights);
                    OnPropertyChanged(nameof(FilteredFlights));


                    SelectedFlightForDelete = null; // resetuj izbor
                }
                catch (Exception ex)
                {
                    DeleteErrorMessage = "Error deleting flight: " + ex.Message;
                    log.Error("Exception deleting flight", ex);
                }
            }
        }

        private bool CanUndoOperation(object obj)
        {
            return !CommandManager.IsUndoStackEmpty();
        }

        private void UndoOperation(object obj)
        {
            log.Info("Undoing last operation...");
            CommandManager.Undo();

            FilteredFlights = new ObservableCollection<Flight>(Flights);
            OnPropertyChanged(nameof(FilteredFlights));
            log.Info("Undo operation completed.");
        }

        private bool CanRedoOperation(object obj)
        {
            return !CommandManager.IsRedoStackEmpty();
        }

        private void RedoOperation(object obj)
        {
            log.Info("Redoing last operation...");
            CommandManager.Redo();

            FilteredFlights = new ObservableCollection<Flight>(Flights);
            OnPropertyChanged(nameof(FilteredFlights));
            log.Info("Redo operation completed.");
        }
        private void SearchFlights(object obj)
        {
            log.Info($"Searching flights by {SelectedSearchProperty} with value '{SearchText}'");
            if (string.IsNullOrWhiteSpace(SearchText) || string.IsNullOrWhiteSpace(SelectedSearchProperty))
            {
                // ako nema unosa ili nije izabrano polje, prikaži sve letove
                FilteredFlights = new ObservableCollection<Flight>(Flights);
                return;
            }

            var lower = SearchText.ToLower();

            var filtered = new List<Flight>();

            foreach (Flight f in Flights)
            {
                bool match = false;

                if (SelectedSearchProperty == "Flight Number")
                    match = f.FlightNumber != null && f.FlightNumber.ToLower().Equals(lower);
                else if (SelectedSearchProperty == "Departure")
                    match = f.Departure != null && f.Departure.Name.ToLower().Equals(lower);
                else if (SelectedSearchProperty == "Arrival")
                    match = f.Arrival != null && f.Arrival.Name.ToLower().Equals(lower);
                else if (SelectedSearchProperty == "Airplane")
                    match = f.Airplane != null && f.Airplane.Name.ToLower().Equals(lower);
                else if (SelectedSearchProperty == "Type")
                    match = f.Type.ToString().ToLower().Equals(lower);
                else if (SelectedSearchProperty == "State")
                    match = f.State != null && f.State.Name.ToLower().Equals(lower);

                if (match)
                    filtered.Add(f);
            }

            FilteredFlights = new ObservableCollection<Flight>(filtered);
            OnPropertyChanged(nameof(FilteredFlights));
            log.Info($"Search completed. {FilteredFlights.Count} flights found.");
        }
    }
}
