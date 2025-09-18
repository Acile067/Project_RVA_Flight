using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace RVA_Flight.Server.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CityService : ICityService
    {
        private readonly IStorageService _storageService;
        private static readonly ILog log = LogManager.GetLogger(typeof(CityService));

        public CityService(IStorageService storageService)
        {
            _storageService = storageService;
            log.Info("CityService initialized.");
        }

        public List<City> LoadCities()
        {
            try
            {
                log.Info("Loading cities...");
                var storage = _storageService.GetStorage();
                var result = storage.Load<List<City>>(_storageService.GetCityFilePath()) ?? new List<City>();
                log.Info($"Loaded {result.Count} cities.");
                return result;
            }
            catch (FileNotFoundException)
            {
                log.Warn("City file not found. Returning empty list.");
                return new List<City>();
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error while loading cities.", ex);
                return new List<City>();
            }
        }

        public void SaveCity(City city)
        {
            if (string.IsNullOrWhiteSpace(city?.Name) || string.IsNullOrWhiteSpace(city?.Country))
            {
                log.Warn("Attempt to save invalid city (missing Name or Country).");
                throw new FaultException("City must have both Name and Country.");
            }

            log.Info($"Saving city: {city.Name}, {city.Country}");
            var storage = _storageService.GetStorage();

            List<City> cities;
            try
            {
                cities = storage.Load<List<City>>(_storageService.GetCityFilePath()) ?? new List<City>(); ;
            }
            catch (FileNotFoundException)
            {
                log.Warn("City file not found. Starting with a new list.");
                cities = new List<City>();
            }

            if (cities.Any(c => c.Name.Equals(city.Name, StringComparison.OrdinalIgnoreCase)))
            {
                log.Warn($"City '{city.Name}' already exists. Save aborted.");
                throw new FaultException($"City '{city.Name}' already exists.");
            }

            cities.Add(city);

            storage.Save(_storageService.GetCityFilePath(), cities);
            log.Info($"City '{city.Name}' saved successfully. Total cities: {cities.Count}");
        }
    }
}
