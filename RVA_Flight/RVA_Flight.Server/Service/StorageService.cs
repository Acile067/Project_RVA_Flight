using log4net;
using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Entities;
using RVA_Flight.Common.Enums;
using RVA_Flight.Server.DataStorage;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string _airplaneFilePath;
        private string _charterFlightFilePath;

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

        public string CharterFlightFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_charterFlightFilePath))
                {
                    log.Warn("CharterFlightFilePath requested but storage type not selected.");
                    throw new InvalidOperationException("Storage type not selected.");
                }
                return _charterFlightFilePath;
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
                    _charterFlightFilePath = "charterFlight.csv";
                    break;
                case StorageType.Json:
                    _flightFilePath = "flight.json";
                    _cityFilePath = "city.json";
                    _airplaneFilePath = "airplane.json";
                    _charterFlightFilePath = "charterFlight.json";
                    break;
                case StorageType.Xml:
                    _flightFilePath = "flight.xml";
                    _cityFilePath = "city.xml";
                    _airplaneFilePath = "airplane.xml";
                    _charterFlightFilePath = "charterFlight.xml";
                    break;
            }

            if (AreAllFilesEmptyOrMissing())
            {
                InitializeDefaultData();
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
        public string GetCharterFlightFilePath() => CharterFlightFilePath;
        private bool AreAllFilesEmptyOrMissing()
        {
            string[] files = { _flightFilePath, _cityFilePath, _airplaneFilePath, _charterFlightFilePath };
            foreach (var file in files)
            {
                if (File.Exists(file) && new FileInfo(file).Length > 0)
                {
                    return false;
                }
            }
            return true;
        }
        private void InitializeDefaultData()
        {
            // 3 test grada
            var cities = new List<City>
            {
                new City { Name = "New York", Country = "USA" },
                new City { Name = "London", Country = "UK" },
                new City { Name = "Tokyo", Country = "Japan" }
            };
            _storage.Save(_cityFilePath, cities);

            // 3 test aviona
            var airplanes = new List<Airplane>
            {
                new Airplane { Code = "AA320", Name = "Airbus A320", Capacity = 180, YearOfManufacture = 2015 },
                new Airplane { Code = "BB737", Name = "Boeing 737", Capacity = 160, YearOfManufacture = 2016 },
                new Airplane { Code = "EE190", Name = "Embraer 190", Capacity = 100, YearOfManufacture = 2018 }
            };
            _storage.Save(_airplaneFilePath, airplanes);

            // 3 test charter leta
            var charterFlights = new List<CharterFlight>
            {
                new CharterFlight("CFF101", FlightType.PRIVATE, DateTime.Now.AddHours(-5), DateTime.Now),
                new CharterFlight("CFF102", FlightType.PUBLIC, DateTime.Now.AddHours(-3), DateTime.Now.AddHours(2)),
                new CharterFlight("CFF103", FlightType.PRIVATE, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(1))
            };
            _storage.Save(_charterFlightFilePath, charterFlights);

            // 3 test regularna leta
            var flights = new List<Flight>
            {
                new Flight
                {
                    FlightNumber = "FLL100",
                    Type = FlightType.PUBLIC,
                    DepartureTime = DateTime.Now.AddHours(-4),
                    ArrivalTime = DateTime.Now,
                    PilotMessage = "Smooth flight",
                    DelayMinutes = 0,
                    Departure = cities[0],
                    Arrival = cities[1],
                    Airplane = airplanes[0],
                    StateName = "On Time"
                },
                new Flight
                {
                    FlightNumber = "FLL200",
                    Type = FlightType.PRIVATE,
                    DepartureTime = DateTime.Now.AddHours(-6),
                    ArrivalTime = DateTime.Now.AddHours(-1),
                    PilotMessage = "Minor turbulence",
                    DelayMinutes = 10,
                    Departure = cities[1],
                    Arrival = cities[2],
                    Airplane = airplanes[1],
                    StateName = "Delayed"
                },
                new Flight
                {
                    FlightNumber = "FLL300",
                    Type = FlightType.PUBLIC,
                    DepartureTime = DateTime.Now.AddHours(-2),
                    ArrivalTime = DateTime.Now.AddHours(1),
                    PilotMessage = "On schedule",
                    DelayMinutes = 0,
                    Departure = cities[2],
                    Arrival = cities[0],
                    Airplane = airplanes[2],
                    StateName = "On Time"
                }
            };
            _storage.Save(_flightFilePath, flights);

            log.Info("Initialized all data files with 3 default instances each.");
        }
    }
}
