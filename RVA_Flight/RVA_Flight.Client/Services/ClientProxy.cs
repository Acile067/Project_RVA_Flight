using RVA_Flight.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Client.Services
{
    public class ClientProxy
    {
        private static ClientProxy _instance;
        private static readonly object _lock = new object();

        private IFlightService _flightService;
        private IStorageService _storageService;
        private ICityService _cityService;
        private IAirplaneService _airplaneService;

        public static ClientProxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new ClientProxy();
                    }
                }
                return _instance;
            }
        }

        private ClientProxy()
        {
            // FlightService endpoint
            var flightFactory = new ChannelFactory<IFlightService>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost:5000/FlightService"));
            _flightService = flightFactory.CreateChannel();

            // StorageService endpoint
            var storageFactory = new ChannelFactory<IStorageService>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost:5001/StorageService"));
            _storageService = storageFactory.CreateChannel();

            // CityService endpoint
            var cityFactory = new ChannelFactory<ICityService>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost:5002/CityService"));
            _cityService = cityFactory.CreateChannel();

            // AirplaneService endpoint
            var airplaneFactory = new ChannelFactory<IAirplaneService>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost:5003/AirplaneService"));
            _airplaneService = airplaneFactory.CreateChannel();
        }

        public IFlightService FlightService => _flightService;
        public IStorageService StorageService => _storageService;
        public ICityService CityService => _cityService;
        public IAirplaneService AirplaneService => _airplaneService;
    }
}
