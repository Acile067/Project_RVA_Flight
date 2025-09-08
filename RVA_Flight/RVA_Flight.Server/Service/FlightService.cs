using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Entities;
using RVA_Flight.Common.Enums;
using RVA_Flight.Server.DataStorage;
using System;
using System.Collections.Generic;
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

        public void SaveFlight(FlightDto flight)
        {
            var storage = _storageService.GetStorage();
            storage.Save(_storageService.GetFlightFilePath(), flight);
        }

        public FlightDto LoadFlight()
        {
            var storage = _storageService.GetStorage();
            return storage.Load<FlightDto>(_storageService.GetFlightFilePath());
        }
    }
}
