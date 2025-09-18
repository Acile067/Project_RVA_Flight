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

        public override string GetPilotMessage() => $"Flight delayed by {flight.DelayMinutes} minutes.";
        public override int GetDelayMinutes() => flight.DelayMinutes;
    }
}
