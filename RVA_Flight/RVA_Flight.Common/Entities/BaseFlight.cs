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
    public abstract class BaseFlight
    {
        protected string flightNumber;
        protected FlightType type;


        [DataMember]
        public string FlightNumber
        {
            get => flightNumber;
            set => flightNumber = value;
        }

        [DataMember]
        public FlightType Type
        {
            get => type;
            set => type = value;
        }


    }
}
