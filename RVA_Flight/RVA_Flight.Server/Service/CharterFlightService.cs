using log4net;
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
    public class CharterFlightService : ICharterFlight
    {
        private readonly IStorageService _storageService;
        private static readonly ILog log = LogManager.GetLogger(typeof(CityService));

        public CharterFlightService(IStorageService storageService)
        {
            _storageService = storageService;
            log.Info("CharterFlightService initialized.");
        }
        public List<CharterFlight> LoadCharterFlights()
        {
            try
            {
                log.Info("Loading charter flights...");
                var storage = _storageService.GetStorage();
                var result = storage.Load<List<CharterFlight>>(_storageService.GetCharterFlightFilePath())
                             ?? new List<CharterFlight>();
                log.Info($"Loaded {result.Count} charter flights.");
                return result;
            }
            catch (FileNotFoundException)
            {
                log.Warn("Charter flight file not found. Returning empty list.");
                return new List<CharterFlight>();
            }
            catch (Exception ex)
            {
                log.Error("Error loading charter flights: " + ex.Message, ex);
                return new List<CharterFlight>();
            }
        }

        public void SaveCharterFlight(CharterFlight charterFlight)
        {
            if (string.IsNullOrWhiteSpace(charterFlight?.FlightNumber))
            {
                log.Warn("Attempt to save invalid charter flight (missing FlightNumber).");
                throw new FaultException("Charter flight must have a FlightNumber.");
            }

            log.Info($"Saving charter flight: {charterFlight.FlightNumber}");
            var storage = _storageService.GetStorage();

            List<CharterFlight> flights;
            try
            {
                flights = storage.Load<List<CharterFlight>>(_storageService.GetCharterFlightFilePath())
                          ?? new List<CharterFlight>();
            }
            catch (FileNotFoundException)
            {
                log.Warn("Charter flight file not found. Starting with a new list.");
                flights = new List<CharterFlight>();
            }

            if (flights.Any(f => f.FlightNumber.Equals(charterFlight.FlightNumber, StringComparison.OrdinalIgnoreCase)))
            {
                log.Warn($"Charter flight '{charterFlight.FlightNumber}' already exists. Save aborted.");
                throw new FaultException($"Charter flight '{charterFlight.FlightNumber}' already exists.");
            }

            flights.Add(charterFlight);
            storage.Save(_storageService.GetCharterFlightFilePath(), flights);
            log.Info($"Charter flight '{charterFlight.FlightNumber}' saved successfully. Total flights: {flights.Count}");
        }
    }
}
