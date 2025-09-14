using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.State
{
    public abstract class FlightState
    {
        protected Flight flight;
        public abstract string Name { get; }
        public void setFlight(Flight flight)
        {
            this.flight = flight;
        }
        public abstract void Handle();
    }
}
