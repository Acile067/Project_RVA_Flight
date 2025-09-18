using RVA_Flight.Client.Services;
using RVA_Flight.Common.Contracts;
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
    public class StorageSelectionViewModel : BaseViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StorageSelectionViewModel));

        private readonly MainWindowViewModel _mainWindow;

        public StorageSelectionViewModel(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;

            StorageOptions = new ObservableCollection<string> { "Csv", "Json", "Xml" };

            SelectStorageCommand = new RelayCommand(SelectStorage);

            log.Info("StorageSelectionViewModel initialized with options: Csv, Json, Xml");
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
                log.Info($"Selected storage changed to: {_selectedStorage}");
            }
        }

        public ICommand SelectStorageCommand { get; }

        private void SelectStorage(object obj)
        {
            if (string.IsNullOrEmpty(SelectedStorage))
            {
                log.Warn("Attempted to select storage, but no option was chosen.");
                return;
            }

            try
            {
                log.Info($"Selecting storage type: {SelectedStorage}");
                ClientProxy.Instance.StorageService.SelectStorage(SelectedStorage);
                log.Info($"Storage type '{SelectedStorage}' selected successfully.");

                _mainWindow.CurrentViewModel = new FlightViewModel();
                log.Info("FlightViewModel loaded after storage selection.");
            }
            catch (FaultException ex)
            {
                log.Warn($"FaultException when selecting storage: {ex.Message}");
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred while selecting storage.", ex);
            }
        }
    }
}
