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

namespace RVA_Flight.Client.ViewModels
{
    public class AirplaneViewModel : BaseViewModel
    {
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
            Airplanes = new ObservableCollection<Airplane>(
                ClientProxy.Instance.AirplaneService.LoadAirplanes() ?? new List<Airplane>()
            );

            NewAirplane = new Airplane();
            AddAirplaneCommand = new RelayCommand(AddAirplane);
        }

        private void AddAirplane(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewAirplane?.Name) ||
                string.IsNullOrWhiteSpace(NewAirplane?.Code) ||
                NewAirplane.Capacity <= 0 ||
                NewAirplane.YearOfManufacture <= 0)
            {
                ErrorMessage = "All fields must be provided and valid.";
                return;
            }

            try
            {
                ErrorMessage = "";

                ClientProxy.Instance.AirplaneService.SaveAirplane(NewAirplane);
                Airplanes.Add(new Airplane
                {
                    Name = NewAirplane.Name,
                    Code = NewAirplane.Code,
                    Capacity = NewAirplane.Capacity,
                    YearOfManufacture = NewAirplane.YearOfManufacture
                });

                NewAirplane = new Airplane();
                OnPropertyChanged(nameof(NewAirplane));
            }
            catch (FaultException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error saving airplane: " + ex.Message;
            }
        }
    }
}
