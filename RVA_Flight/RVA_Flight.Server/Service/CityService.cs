using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Entities;
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
    public class CityService : ICityService
    {
        private readonly IStorageService _storageService;

        public CityService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public List<City> LoadCities()
        {
            try
            {
                var storage = _storageService.GetStorage();
                return storage.Load<List<City>>(_storageService.GetCityFilePath()) ?? new List<City>();
            }
            catch (FileNotFoundException)
            {
                return new List<City>();
            }
            catch (Exception ex)
            {
                return new List<City>();
            }
        }

        public void SaveCity(City city)
        {
            if (string.IsNullOrWhiteSpace(city?.Name) || string.IsNullOrWhiteSpace(city?.Country))
            {
                throw new FaultException("City must have both Name and Country.");
            }

            var storage = _storageService.GetStorage();

            List<City> cities;
            try
            {
                cities = storage.Load<List<City>>(_storageService.GetCityFilePath()) ?? new List<City>(); ;
            }
            catch (FileNotFoundException)
            {
                cities = new List<City>();
            }

            if (cities.Any(c => c.Name.Equals(city.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new FaultException($"City '{city.Name}' already exists.");
            }

            cities.Add(city);

            storage.Save(_storageService.GetCityFilePath(), cities);
        }
    }
}
