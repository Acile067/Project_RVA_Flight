using CsvHelper.Configuration;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Server.Mappings
{
    public class FlightMap : ClassMap<Flight>
    {
        public FlightMap()
        {
            Map(f => f.FlightNumber);
            Map(f => f.DepartureTime);
            Map(f => f.ArrivalTime);
            Map(f => f.DealayMinutes);
            Map(f => f.Type);
            Map(f => f.PilotMessage);
            Map(f => f.StateName);

            References<CityMap>(f => f.Departure).Prefix("Departure_");
            References<CityMap>(f => f.Arrival).Prefix("Arrival_");
            References<AirplaneMap>(f => f.Airplane).Prefix("Airplane_");
        }
    }
}
