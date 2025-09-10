using RVA_Flight.Common.Contracts;
using RVA_Flight.Server.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Uri flightBaseAddress = new Uri("http://localhost:5000/FlightService");
            Uri storageBaseAddress = new Uri("http://localhost:5001/StorageService");
            Uri cityBaseAddress = new Uri("http://localhost:5002/CityService");
            Uri airplaneBaseAddress = new Uri("http://localhost:5003/AirplaneService");

            var storageService = new StorageService();
            var flightService = new FlightService(storageService);
            var cityService = new CityService(storageService);
            var airplaneService = new AirplaneService(storageService);

            using (ServiceHost flightHost = new ServiceHost(flightService, flightBaseAddress))
            using (ServiceHost storageHost = new ServiceHost(storageService, storageBaseAddress))
            using (ServiceHost cityHost = new ServiceHost(cityService, cityBaseAddress))
            using (ServiceHost airplaneHost = new ServiceHost(airplaneService, airplaneBaseAddress))
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

                try
                {
                    flightHost.Open();
                    storageHost.Open();
                    cityHost.Open();
                    airplaneHost.Open();

                    Console.WriteLine("WCF services are running:");
                    Console.WriteLine("FlightService: " + flightBaseAddress);
                    Console.WriteLine("StorageService: " + storageBaseAddress);
                    Console.WriteLine("CityService: " + cityBaseAddress);
                    Console.WriteLine("AirplaneService: " + airplaneBaseAddress);
                    Console.WriteLine("Press Enter to exit...");
                    Console.ReadLine();

                    flightHost.Close();
                    storageHost.Close();
                    cityHost.Close();
                    airplaneHost.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    flightHost.Abort();
                    storageHost.Abort();
                }
            }
        }
    }
}
