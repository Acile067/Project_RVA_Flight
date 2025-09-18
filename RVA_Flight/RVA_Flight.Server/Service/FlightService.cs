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
using log4net;

namespace RVA_Flight.Server.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FlightService : IFlightService
    {
        private readonly IStorageService _storageService;
        private static readonly ILog log = LogManager.GetLogger(typeof(FlightService));

        public FlightService(IStorageService storageService)
        {
            _storageService = storageService;
            log.Info("FlightService initialized.");
        }

        public List<Flight> LoadFlights()
        {
            try
            {
                log.Info("Loading flights...");
                var storage = _storageService.GetStorage();
                var result = storage.Load<List<Flight>>(_storageService.GetFlightFilePath()) ?? new List<Flight>();
                log.Info($"Loaded {result.Count} flights.");
                return result;
            }
            catch (FileNotFoundException)
            {
                log.Warn("Flight file not found. Returning empty list.");
                return new List<Flight>();
            }
            catch (Exception ex)
            {
                log.Error("Error loading flights: " + ex.Message, ex);
                return new List<Flight>();
            }
        }

        public void SaveFlight(Flight flight)
        {
            var storage = _storageService.GetStorage();
            log.Info($"Saving flight: {flight?.FlightNumber}");

            List<Flight> flights;
            try
            {
                flights = storage.Load<List<Flight>>(_storageService.GetFlightFilePath()) ?? new List<Flight>(); ;
            }
            catch (FileNotFoundException)
            {
                log.Warn("Flight file not found. Creating new list.");
                flights = new List<Flight>();
            }

            flights.Add(flight);

            storage.Save(_storageService.GetFlightFilePath(), flights);
            log.Info($"Flight '{flight.FlightNumber}' saved successfully.");
        }

        public void DeleteFlight(Flight flight)
        {
            log.Info($"Deleting flight: {flight?.FlightNumber}");
            var storage = _storageService.GetStorage();

            List<Flight> flights;
            try
            {
                flights = storage.Load<List<Flight>>(_storageService.GetFlightFilePath()) ?? new List<Flight>(); ;
            }
            catch (FileNotFoundException)
            {
                log.Warn("Flight file not found. Nothing to delete.");
                flights = new List<Flight>();
            }

            var flightToRemove = flights.FirstOrDefault(f => f.FlightNumber == flight.FlightNumber);

            if (flightToRemove != null)
            {
                flights.Remove(flightToRemove);
                log.Info($"Flight '{flight.FlightNumber}' deleted successfully.");
            }
            else
            {
                log.Warn($"Flight '{flight.FlightNumber}' not found. Nothing deleted.");
            }


            storage.Save(_storageService.GetFlightFilePath(), flights);
        }
    }
}
