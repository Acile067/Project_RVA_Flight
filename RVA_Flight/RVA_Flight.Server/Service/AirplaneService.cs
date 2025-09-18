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
    public class AirplaneService : IAirplaneService
    {
        private readonly IStorageService _storageService;
        private static readonly ILog log = LogManager.GetLogger(typeof(AirplaneService));

        public AirplaneService(IStorageService storageService)
        {
            _storageService = storageService;
            log.Info("AirplaneService initialized.");
        }
        public List<Airplane> LoadAirplanes()
        {
            try
            {
                log.Info("Loading airplanes...");
                var storage = _storageService.GetStorage();
                var result = storage.Load<List<Airplane>>(_storageService.GetAirplaneFilePath()) ?? new List<Airplane>();
                log.Info($"Loaded {result.Count} airplanes.");
                return result;
            }
            catch (FileNotFoundException)
            {
                log.Warn("Airplane file not found. Returning empty list.");
                return new List<Airplane>();
            }
            catch (Exception ex)
            {
                log.Error("Error loading airplanes: " + ex.Message, ex);
                return new List<Airplane>();
            }
        }

        public void SaveAirplane(Airplane airplane)
        {
            if (string.IsNullOrWhiteSpace(airplane?.Name) ||
                string.IsNullOrWhiteSpace(airplane?.Code) ||
                airplane.Capacity == 0 ||
                airplane.YearOfManufacture == 0)
            {
                log.Warn("Invalid airplane data provided.");
                throw new FaultException("Airplane must have all fields not null or empty.");
            }

            if (airplane.Capacity <= 0)
            {
                log.Warn("Capacity must be a positive integer.");
                throw new FaultException("Capacity must be a positive integer.");
            }

            if (airplane.YearOfManufacture <= 0)
            {
                log.Warn("Year of manufacture must be a positive integer.");
                throw new FaultException("Year of manufacture must be a positive integer.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(airplane.Code, @"^[A-Za-z]{2}[A-Za-z0-9]{0,3}$"))
            {
                log.Warn($"Airplane code '{airplane.Code}' is invalid.");
                throw new FaultException("Code must start with two letters followed by up to three letters or digits (max length 5).");
            }

            var storage = _storageService.GetStorage();

            List<Airplane> airplanes;
            try
            {
                airplanes = storage.Load<List<Airplane>>(_storageService.GetAirplaneFilePath()) ?? new List<Airplane>(); ;
            }
            catch (FileNotFoundException)
            {
                log.Warn("Airplane file not found. Creating new list.");
                airplanes = new List<Airplane>();
            }

            if (airplanes.Any(a => a.Code.Equals(airplane.Code, StringComparison.OrdinalIgnoreCase)))
            {
                log.Warn($"Airplane with code '{airplane.Code}' already exists.");
                throw new FaultException($"Airplane with code '{airplane.Code}' already exists.");
            }

            airplanes.Add(airplane);

            storage.Save(_storageService.GetAirplaneFilePath(), airplanes);

            log.Info($"Airplane '{airplane.Name}' ({airplane.Code}) saved successfully.");
        }
    }
}
