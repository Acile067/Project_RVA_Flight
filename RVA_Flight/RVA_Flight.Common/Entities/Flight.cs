using RVA_Flight.Common.Enums;
using RVA_Flight.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RVA_Flight.Common.Entities
{
    [DataContract]
    public class Flight
    {
        [DataMember]
        public virtual string FlightNumber { get; set; }

        [DataMember]
        public virtual FlightType Type { get; set; }

        [DataMember]
        public virtual DateTime ArrivalTime { get; set; }

        [DataMember]
        public  virtual DateTime? DepartureTime { get; set; }

        [DataMember]
        public virtual string PilotMessage { get; set; }

        [DataMember]
        public virtual int DelayMinutes { get; set; }

        [DataMember]
        public virtual Airplane Airplane { get; set; }

        [DataMember]
        public virtual City Arrival { get; set; }

        [DataMember]
        public virtual City Departure { get; set; }

        [DataMember]
        public virtual string StateName
        {
            get { return State?.Name; }
            set
            {
                if (value == "On Time")
                    State = new OnTimeState();
                else if (value == "Delayed")
                    State = new DelayedState();
                else
                    State = null;
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public FlightState State { get; set; }
        public Flight() { }
    }
}
