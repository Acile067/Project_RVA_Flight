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

        // Singleton instanca
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
            var factory = new ChannelFactory<IFlightService>(new BasicHttpBinding(),
                new EndpointAddress("http://localhost:5000/FlightService"));
            _flightService = factory.CreateChannel();
        }

        public IFlightService FlightService => _flightService;
    }
}
