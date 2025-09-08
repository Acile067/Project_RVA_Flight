using RVA_Flight.Common.Contracts;
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
    public class StorageSelectionViewModel : BaseViewModel
    {
        private IFlightService _flightService;
        private readonly MainWindowViewModel _mainWindow;

        public StorageSelectionViewModel(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;

            ChannelFactory<IFlightService> factory =
                new ChannelFactory<IFlightService>(new BasicHttpBinding(),
                new EndpointAddress("http://localhost:5000/FlightService"));
            _flightService = factory.CreateChannel();

            StorageOptions = new ObservableCollection<string> { "Csv", "Json", "Xml" };
            SelectStorageCommand = new RelayCommand(SelectStorage);
        }

        public ObservableCollection<string> StorageOptions { get; set; }

        private string _selectedStorage;
        public string SelectedStorage
        {
            get => _selectedStorage;
            set
            {
                _selectedStorage = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectStorageCommand { get; }

        private void SelectStorage(object obj)
        {
            if (string.IsNullOrEmpty(SelectedStorage)) return;

            _flightService.SelectStorage(SelectedStorage);

            _mainWindow.CurrentViewModel = new FlightViewModel();
        }
    }
}
