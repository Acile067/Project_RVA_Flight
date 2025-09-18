using RVA_Flight.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.Entities
{
    public class CharterFlight:BaseFlight
    {
        private DateTime arrivalTime;
        private TimeSpan duration;

        [DataMember]
        public DateTime ArrivalTime
        {
            get => arrivalTime;
            set => arrivalTime = value;
        }

        [DataMember]
        public TimeSpan Duration
        {
            get => duration;
            set => duration = value;

        }
        public CharterFlight(string flightNumber, FlightType type, DateTime departureTime, DateTime arrivalTime)
        {
            this.flightNumber = flightNumber;
            this.type = type;
            this.ArrivalTime = arrivalTime;
            this.Duration = arrivalTime - departureTime;
        }
        public CharterFlight() { }
    }
}
