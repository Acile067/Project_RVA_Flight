using log4net;
using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using RVA_Flight.Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RVA_Flight.Client.ViewModels
{
    public class CharterFlightViewModel : BaseViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CharterFlightViewModel));

        public ObservableCollection<CharterFlight> CharterFlights { get; set; }

        public CharterFlight NewCharterFlight { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        public double NewCharterFlightDurationHours
        {
            get => NewCharterFlight?.Duration.TotalHours ?? 0;
            set
            {
                if (NewCharterFlight != null)
                {
                    NewCharterFlight.Duration = TimeSpan.FromHours(value);
                    OnPropertyChanged(nameof(NewCharterFlightDurationHours));
                }
            }
        }

        public ICommand AddCharterFlightCommand { get; }

        public CharterFlightViewModel()
        {
            log.Info("Initializing CharterFlightViewModel...");

            CharterFlights = new ObservableCollection<CharterFlight>(
                ClientProxy.Instance.CharterFlightService.LoadCharterFlights() ?? new System.Collections.Generic.List<CharterFlight>()
            );

            NewCharterFlight = new CharterFlight
            {
                ArrivalTime = DateTime.Now.AddHours(1),
                Duration = TimeSpan.FromHours(1),
                Type = FlightType.PRIVATE
            };

            AddCharterFlightCommand = new RelayCommand(AddCharterFlight);

            log.Info("CharterFlightViewModel initialized successfully.");
        }

        private void AddCharterFlight(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewCharterFlight?.FlightNumber))
            {
                ErrorMessage = "Flight Number is required.";
                log.Warn("Attempted to add charter flight with missing FlightNumber.");
                return;
            }

            if (CharterFlights.Any(f => f.FlightNumber.Equals(NewCharterFlight.FlightNumber, StringComparison.OrdinalIgnoreCase)))
            {
                ErrorMessage = $"Charter flight '{NewCharterFlight.FlightNumber}' already exists.";
                log.Warn(ErrorMessage);
                return;
            }

            try
            {
                ErrorMessage = "";
                ClientProxy.Instance.CharterFlightService.SaveCharterFlight(NewCharterFlight);
                CharterFlights.Add(new CharterFlight
                {
                    FlightNumber = NewCharterFlight.FlightNumber,
                    Type = NewCharterFlight.Type,
                    ArrivalTime = NewCharterFlight.ArrivalTime,
                    Duration = NewCharterFlight.Duration
                });

                log.Info($"Charter flight '{NewCharterFlight.FlightNumber}' added successfully.");

                NewCharterFlight = new CharterFlight
                {
                    ArrivalTime = DateTime.Now.AddHours(1),
                    Duration = TimeSpan.FromHours(1)
                };
                OnPropertyChanged(nameof(NewCharterFlight));
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error saving charter flight: " + ex.Message;
                log.Error("Exception adding charter flight", ex);
            }
        }
    }
}
