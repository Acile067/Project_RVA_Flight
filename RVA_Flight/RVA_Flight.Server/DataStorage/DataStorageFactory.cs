using RVA_Flight.Common.Enums;
using RVA_Flight.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server.DataStorage
{
    public class DataStorageFactory
    {
        public static IDataStorage Create(StorageType type)
        {
            switch (type)
            {
                case StorageType.Csv:
                    return new CsvDataStorage();
                case StorageType.Json:
                    return new JsonDataStorage();
                case StorageType.Xml:
                    return new XmlDataStorage();
                default:
                    throw new ArgumentException("Unsupported storage type");
            }
        }
    }
}
