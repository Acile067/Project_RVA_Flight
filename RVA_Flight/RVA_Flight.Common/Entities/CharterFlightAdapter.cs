using RVA_Flight.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.Entities
{
    [DataContract]
    public class CharterFlightAdapter : Flight
    {
        private CharterFlight charterFlight;

        public CharterFlightAdapter(CharterFlight charterFlight)
        {
            this.charterFlight = charterFlight;
        }

        [DataMember]
        public override string FlightNumber
        {
            get => charterFlight.FlightNumber;
            set => charterFlight.FlightNumber = value;
        }

        [DataMember]
        public override FlightType Type
        {
            get => charterFlight.Type;
            set => charterFlight.Type = value;
        }

        [DataMember]
       

        public override DateTime ArrivalTime
        {
            get => charterFlight.ArrivalTime;
            set => charterFlight.ArrivalTime = value;
        }

        [DataMember]
        public TimeSpan Duration
        {
            get => charterFlight.Duration;
            set => charterFlight.Duration = value;
        }



    }
}
