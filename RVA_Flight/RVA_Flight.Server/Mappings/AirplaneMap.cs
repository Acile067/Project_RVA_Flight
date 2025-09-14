using CsvHelper.Configuration;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server.Mappings
{
    public class AirplaneMap : ClassMap<Airplane>
    {
        public AirplaneMap()
        {
            Map(a => a.Code);
            Map(a => a.Name);
            Map(a => a.Capacity);
            Map(a => a.YearOfManufacture);
        }
    }
}
