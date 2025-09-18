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
    public class CityViewModel : BaseViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CityViewModel));
        public ObservableCollection<City> Cities { get; set; }
        public City NewCity { get; set; }

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

        public ICommand AddCityCommand { get; }

        public CityViewModel()
        {
            try
            {
                log.Info("Loading cities from service...");
                var loadedCities = ClientProxy.Instance.CityService.LoadCities() ?? new List<City>();
                Cities = new ObservableCollection<City>(loadedCities);
                log.Info($"Loaded {Cities.Count} cities.");
            }
            catch (Exception ex)
            {
                log.Error("Error loading cities", ex);
                Cities = new ObservableCollection<City>();
            }

            NewCity = new City();
            AddCityCommand = new RelayCommand(AddCity);
        }

        private void AddCity(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewCity?.Name) || string.IsNullOrWhiteSpace(NewCity?.Country))
            {
                ErrorMessage = "Both Name and Country must be provided.";
                log.Warn("User tried to add a city with missing Name or Country.");
                return;
            }

            try
            {
                ErrorMessage = "";
                log.Info($"Adding new city: {NewCity.Name}, {NewCity.Country}");
                ClientProxy.Instance.CityService.SaveCity(NewCity);
                Cities.Add(new City { Name = NewCity.Name, Country = NewCity.Country });
                log.Info($"City '{NewCity.Name}' added successfully.");

                NewCity = new City();
                OnPropertyChanged(nameof(NewCity));
            }
            catch (FaultException ex)
            {
                ErrorMessage = ex.Message;
                log.Warn($"FaultException when adding city: {ex.Message}");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error saving city: " + ex.Message;
                log.Error("Exception when saving city", ex);
            }
        }
    }
}
