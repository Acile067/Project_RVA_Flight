using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Entities;
using RVA_Flight.Server.Contracts;
using RVA_Flight.Server.DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FlightService : IFlightService
    {
        private IDataStorage _storage;
        private string _flightFilePath;
        private string _cityFilePath;

        public FlightService()
        {
            _storage = DataStorageFactory.Create(StorageType.Csv);
        }

        public string SelectStorage(string storageType)
        {
            if (!Enum.TryParse(storageType, true, out StorageType type))
                return $"Invalid storage type: {storageType}";

            switch (type)
            {
                case StorageType.Csv:
                    _flightFilePath = "flight.csv";
                    _cityFilePath = "city.csv";
                    break;
                case StorageType.Json:
                    _flightFilePath = "flight.json";
                    _cityFilePath = "city.json";
                    break;
                case StorageType.Xml:
                    _flightFilePath = "flight.xml";
                    _cityFilePath = "city.xml";
                    break;
                default:
                    return $"Unsupported storage type: {storageType}";
            }

            _storage = DataStorageFactory.Create(type);
            return $"Selected storage: {type}";
        }

        public void SaveFlight(FlightDto flight)
        {
            _storage.Save(_flightFilePath, flight);
        }

        public FlightDto LoadFlight()
        {
            return _storage.Load<FlightDto>(_flightFilePath);
        }
    }
}
