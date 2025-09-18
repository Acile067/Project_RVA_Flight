using RVA_Flight.Common.Contracts;
using RVA_Flight.Server.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace RVA_Flight.Server
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            log.Info("=== App started ===");
            Uri flightBaseAddress = new Uri("http://localhost:5000/FlightService");
            Uri storageBaseAddress = new Uri("http://localhost:5001/StorageService");
            Uri cityBaseAddress = new Uri("http://localhost:5002/CityService");
            Uri airplaneBaseAddress = new Uri("http://localhost:5003/AirplaneService");
            Uri charterFlightBaseAddress = new Uri("http://localhost:5004/CharterFlightService");

            var storageService = new StorageService();
            var flightService = new FlightService(storageService);
            var cityService = new CityService(storageService);
            var airplaneService = new AirplaneService(storageService);
            var charterFlightService = new CharterFlightService(storageService);

            using (ServiceHost flightHost = new ServiceHost(flightService, flightBaseAddress))
            using (ServiceHost storageHost = new ServiceHost(storageService, storageBaseAddress))
            using (ServiceHost cityHost = new ServiceHost(cityService, cityBaseAddress))
            using (ServiceHost airplaneHost = new ServiceHost(airplaneService, airplaneBaseAddress))
            using (ServiceHost charterFlightHost = new ServiceHost(charterFlightService, charterFlightBaseAddress))
            {
                // Endpoint-i za FlightService
                flightHost.AddServiceEndpoint(typeof(IFlightService), new BasicHttpBinding(), "");
                flightHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = flightBaseAddress });
                flightHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                // Endpoint-i za StorageService
                storageHost.AddServiceEndpoint(typeof(IStorageService), new BasicHttpBinding(), "");
                storageHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = storageBaseAddress });
                storageHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                // Endpoint-i za CityService
                cityHost.AddServiceEndpoint(typeof(ICityService), new BasicHttpBinding(), "");
                cityHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = cityBaseAddress });
                cityHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                // Endpoint-i za AirplaneService
                airplaneHost.AddServiceEndpoint(typeof(IAirplaneService), new BasicHttpBinding(), "");
                airplaneHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = airplaneBaseAddress });
                airplaneHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                // Endpoint-i za CharterFlightService
                charterFlightHost.AddServiceEndpoint(typeof(ICharterFlight), new BasicHttpBinding(), "");
                charterFlightHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = charterFlightBaseAddress });
                charterFlightHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                try
                {
                    flightHost.Open();
                    storageHost.Open();
                    cityHost.Open();
                    airplaneHost.Open();
                    charterFlightHost.Open();

                    Console.WriteLine("WCF services are running:");
                    Console.WriteLine("FlightService: " + flightBaseAddress);
                    Console.WriteLine("StorageService: " + storageBaseAddress);
                    Console.WriteLine("CityService: " + cityBaseAddress);
                    Console.WriteLine("AirplaneService: " + airplaneBaseAddress);
                    Console.WriteLine("CharterFlightService: " + charterFlightBaseAddress);
                    Console.WriteLine("Press Enter to exit...");

                    Console.ReadLine();

                    flightHost.Close();
                    storageHost.Close();
                    cityHost.Close();
                    airplaneHost.Close();
                    charterFlightHost.Close();

                    log.Info("=== App stopped ===");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    flightHost.Abort();
                    storageHost.Abort();
                    cityHost.Abort();
                    airplaneHost.Abort();
                    charterFlightHost.Abort();
                    log.Error("Exception occurred: ", ex);
                }
            }
        }
    }
}
