using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.State
{
    public class DelayedState : FlightState
    {
        public override string Name => "Delayed";
        public override void Handle()
        {
            Console.WriteLine($"Flight {flight.FlightNumber} is delayed.");
        }
    }
}
