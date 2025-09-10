using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.Entities
{
    [DataContract]
    public class Flight
    {
        private string flightNumber;

        [DataMember]
        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }
    }
}
