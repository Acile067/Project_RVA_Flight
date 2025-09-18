using RVA_Flight.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.Contracts
{
    [ServiceContract]
    public interface IStorageService
    {
        [OperationContract]
        string SelectStorage(string storageType);

        [OperationContract]
        IDataStorage GetStorage();
        [OperationContract]
        string GetFlightFilePath();
        [OperationContract]
        string GetCityFilePath();
        [OperationContract]
        string GetAirplaneFilePath();
        [OperationContract]
        string GetCharterFlightFilePath();
    }
}
