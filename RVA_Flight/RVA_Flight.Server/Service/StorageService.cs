using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Enums;
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
    public class StorageService : IStorageService
    {
        private IDataStorage _storage;
        private string _flightFilePath;
        private string _cityFilePath;

        public string FlightFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_flightFilePath))
                    throw new InvalidOperationException("Storage type not selected.");
                return _flightFilePath;
            }
        }

        public string CityFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_cityFilePath))
                    throw new InvalidOperationException("Storage type not selected.");
                return _cityFilePath;
            }
        }

        public string SelectStorage(string storageType)
        {
            if (!Enum.TryParse(storageType, true, out StorageType type))
                return $"Invalid storage type: {storageType}";

            _storage = DataStorageFactory.Create(type);

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
            }

            return $"Selected storage: {type}";
        }

        public IDataStorage GetStorage()
        {
            if (_storage == null)
                throw new InvalidOperationException("Storage type not selected.");
            return _storage;
        }

        public string GetFlightFilePath() => FlightFilePath;
        public string GetCityFilePath() => CityFilePath;
    }
}
