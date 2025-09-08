using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.Contracts
{
    [ServiceContract]
    public interface IFlightService
    {
        [OperationContract]
        string SelectStorage(string storageType);
        [OperationContract]
        void SaveFlight(FlightDto flight);

        [OperationContract]
        FlightDto LoadFlight();
    }
}
