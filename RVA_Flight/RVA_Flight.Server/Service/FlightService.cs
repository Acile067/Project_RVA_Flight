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
    public class FlightService : IFlightService
    {
        private readonly IStorageService _storageService;

        public FlightService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public List<Flight> LoadFlights()
        {
            try
            {
                var storage = _storageService.GetStorage();
                return storage.Load<List<Flight>>(_storageService.GetFlightFilePath()) ?? new List<Flight>();
            }
            catch (FileNotFoundException)
            {
                return new List<Flight>();
            }
            catch (Exception ex)
            {
                return new List<Flight>();
            }
        }

        public void SaveFlight(Flight flight)
        {
            var storage = _storageService.GetStorage();

            List<Flight> flights;
            try
            {
                flights = storage.Load<List<Flight>>(_storageService.GetFlightFilePath()) ?? new List<Flight>(); ;
            }
            catch (FileNotFoundException)
            {
                flights = new List<Flight>();
            }

            flights.Add(flight);

            storage.Save(_storageService.GetFlightFilePath(), flights);
        }

        public void DeleteFlight(Flight flight)
        {
            var storage = _storageService.GetStorage();

            List<Flight> flights;
            try
            {
                flights = storage.Load<List<Flight>>(_storageService.GetFlightFilePath()) ?? new List<Flight>(); ;
            }
            catch (FileNotFoundException)
            {
                flights = new List<Flight>();
            }

            var flightToRemove = flights.FirstOrDefault(f => f.FlightNumber == flight.FlightNumber);

            if (flightToRemove != null)
            {
                flights.Remove(flightToRemove);
            }
            

            storage.Save(_storageService.GetFlightFilePath(), flights);
        }
    }
}
