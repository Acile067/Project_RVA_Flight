using CsvHelper.Configuration;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server.Mappings
{
    public class CharterFlightMap : ClassMap<CharterFlight>
    {
        public CharterFlightMap()
        {
            Map(f => f.FlightNumber);
            Map(f => f.Type);
            Map(f => f.ArrivalTime).TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
            Map(f => f.Duration).TypeConverterOption.Format("c");
        }
    }
}
