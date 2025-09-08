using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server.Contracts
{
    public interface IDataStorage
    {
        void Save<T>(string filePath, T data);
        T Load<T>(string filePath);
    }
}
