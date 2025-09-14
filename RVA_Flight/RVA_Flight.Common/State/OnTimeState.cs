using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.State
{
    public class OnTimeState : FlightState
    {
        public override string Name => "On Time";
        public override void Handle()
        {
            Console.WriteLine($"Flight {flight.FlightNumber} is on time.");
        }
    }
}
