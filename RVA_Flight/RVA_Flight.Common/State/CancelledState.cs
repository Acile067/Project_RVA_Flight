using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.State
{
    public class CancelledState : FlightState
    {
        public override string Name => "Cancelled";
        public override void Handle()
        {
            Console.WriteLine($"Flight {flight.FlightNumber} is cancelled.");
        }
    }
}
