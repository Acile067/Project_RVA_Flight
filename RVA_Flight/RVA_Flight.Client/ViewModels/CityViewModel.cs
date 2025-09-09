using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RVA_Flight.Client.ViewModels
{
    public class CityViewModel : BaseViewModel
    {
        public ObservableCollection<City> Cities { get; set; }
        public City NewCity { get; set; }

        public ICommand AddCityCommand { get; }

        public CityViewModel()
        {
            Cities = new ObservableCollection<City>(
                ClientProxy.Instance.CityService.LoadCities() ?? new List<City>()
            );

            NewCity = new City();
            AddCityCommand = new RelayCommand(AddCity);
        }

        private void AddCity(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewCity?.Name) || string.IsNullOrWhiteSpace(NewCity?.Country))
                return;

            ClientProxy.Instance.CityService.SaveCity(NewCity);
            Cities.Add(new City { Name = NewCity.Name, Country = NewCity.Country });

            NewCity = new City();
            OnPropertyChanged(nameof(NewCity));
        }
    }
}
