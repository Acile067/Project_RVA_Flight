using CsvHelper.Configuration;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server.Mappings
{
    public class CityMap : ClassMap<City>
    {
        public CityMap()
        {
            Map(c => c.Name);
            Map(c => c.Country);
        }
    }
}
