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
            Uri baseAddress = new Uri("http://localhost:5000/FlightService");

            var serviceInstance = new FlightService();

            using (ServiceHost host = new ServiceHost(serviceInstance, baseAddress))
            {
                host.AddServiceEndpoint(typeof(IFlightService), new BasicHttpBinding(), "");

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpGetUrl = baseAddress
                };
                host.Description.Behaviors.Add(smb);

                host.AddServiceEndpoint(
                    ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexHttpBinding(),
                    "mex"
                );

                try
                {
                    host.Open();
                    Console.WriteLine("WCF server is running at " + baseAddress);
                    Console.WriteLine("Press Enter to exit...");
                    Console.ReadLine();
                    host.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    host.Abort();
                }
            }

        }
    }
}
