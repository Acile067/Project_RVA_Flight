using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RVA_Flight.Client.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowNavigation));
            }
        }

        public ICommand NavigateCommand { get; }
        public bool ShowNavigation => !(CurrentViewModel is StorageSelectionViewModel);
        public MainWindowViewModel()
        {
            CurrentViewModel = new StorageSelectionViewModel(this);

            NavigateCommand = new RelayCommand(Navigate);
        }

        private void Navigate(object destination)
        {
            switch (destination?.ToString())
            {
                case "Flight":
                    CurrentViewModel = new FlightViewModel();
                    break;
                case "City":
                    CurrentViewModel = new CityViewModel();
                    break;
                case "Airplane":
                    CurrentViewModel = new AirplaneViewModel();
                    break;
                case "Simulation":
                    CurrentViewModel = new FlightStateSimulationViewModel();
                    break;
            }
        }
    }
}
