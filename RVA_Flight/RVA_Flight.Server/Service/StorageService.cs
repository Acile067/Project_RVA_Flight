using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Enums;
using RVA_Flight.Server.DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace RVA_Flight.Server.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class StorageService : IStorageService
    {
        private IDataStorage _storage;
        private string _flightFilePath;
        private string _cityFilePath;
        private string _airplaneFilePath;

        private static readonly ILog log = LogManager.GetLogger(typeof(StorageService));

        public string FlightFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_flightFilePath))
                {
                    log.Warn("FlightFilePath requested but storage type not selected.");
                    throw new InvalidOperationException("Storage type not selected.");
                }
                    
                return _flightFilePath;
            }
        }

        public string CityFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_cityFilePath))
                {
                    log.Warn("CityFilePath requested but storage type not selected.");
                    throw new InvalidOperationException("Storage type not selected.");
                }
                return _cityFilePath;
            }
        }

        public string AirplaneFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_airplaneFilePath))
                {
                    log.Warn("AirplaneFilePath requested but storage type not selected.");
                    throw new InvalidOperationException("Storage type not selected.");
                }
                return _airplaneFilePath;
            }
        }

        public string SelectStorage(string storageType)
        {
            if (!Enum.TryParse(storageType, true, out StorageType type))
            {
                log.Warn($"Invalid storage type requested: {storageType}");
                return $"Invalid storage type: {storageType}";
            }

            _storage = DataStorageFactory.Create(type);

            switch (type)
            {
                case StorageType.Csv:
                    _flightFilePath = "flight.csv";
                    _cityFilePath = "city.csv";
                    _airplaneFilePath = "airplane.csv";
                    break;
                case StorageType.Json:
                    _flightFilePath = "flight.json";
                    _cityFilePath = "city.json";
                    _airplaneFilePath = "airplane.json";
                    break;
                case StorageType.Xml:
                    _flightFilePath = "flight.xml";
                    _cityFilePath = "city.xml";
                    _airplaneFilePath = "airplane.xml";
                    break;
            }

            log.Info($"Storage selected: {type} (Flight: {_flightFilePath}, City: {_cityFilePath}, Airplane: {_airplaneFilePath})");
            return $"Selected storage: {type}";
        }

        public IDataStorage GetStorage()
        {
            if (_storage == null)
            {
                log.Error("GetStorage called but storage type not selected.");
                throw new InvalidOperationException("Storage type not selected.");
            }

            log.Info("Returning selected storage instance.");
            return _storage;
        }

        public string GetFlightFilePath() => FlightFilePath;
        public string GetCityFilePath() => CityFilePath;
        public string GetAirplaneFilePath() => AirplaneFilePath;
    }
}
