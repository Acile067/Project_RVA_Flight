using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using log4net;

namespace RVA_Flight.Client.ViewModels
{
    public class AirplaneViewModel : BaseViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AirplaneViewModel));
        public ObservableCollection<Airplane> Airplanes { get; set; }
        public Airplane NewAirplane { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand AddAirplaneCommand { get; }

        public AirplaneViewModel()
        {
            try
            {
                log.Info("Initializing AirplaneViewModel...");

                Airplanes = new ObservableCollection<Airplane>(
                    ClientProxy.Instance.AirplaneService.LoadAirplanes() ?? new List<Airplane>()
                );
                log.Info($"Loaded {Airplanes.Count} airplanes from service.");

                NewAirplane = new Airplane();
                AddAirplaneCommand = new RelayCommand(AddAirplane);

                log.Info("AirplaneViewModel initialized successfully.");
            }
            catch (Exception ex)
            {
                log.Error("Error initializing AirplaneViewModel", ex);
            }
        }

        private void AddAirplane(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewAirplane?.Name) ||
                string.IsNullOrWhiteSpace(NewAirplane?.Code) ||
                NewAirplane.Capacity <= 0 ||
                NewAirplane.YearOfManufacture <= 0)
            {
                ErrorMessage = "All fields must be provided and valid.";
                log.Warn("Attempted to add airplane with missing or invalid fields.");
                return;
            }

            try
            {
                ErrorMessage = "";

                log.Info($"Adding airplane {NewAirplane.Code} - {NewAirplane.Name}...");
                ClientProxy.Instance.AirplaneService.SaveAirplane(NewAirplane);
                Airplanes.Add(new Airplane
                {
                    Name = NewAirplane.Name,
                    Code = NewAirplane.Code,
                    Capacity = NewAirplane.Capacity,
                    YearOfManufacture = NewAirplane.YearOfManufacture
                });
                log.Info($"Airplane {NewAirplane.Code} added successfully.");

                NewAirplane = new Airplane();
                OnPropertyChanged(nameof(NewAirplane));
            }
            catch (FaultException ex)
            {
                ErrorMessage = ex.Message;
                log.Warn($"FaultException adding airplane: {ex.Message}");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error saving airplane: " + ex.Message;
                log.Error("Exception adding airplane", ex);
            }
        }
    }
}
