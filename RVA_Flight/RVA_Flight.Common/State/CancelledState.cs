using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.State
{
    public class CancelledState : FlightState
    {
        public override string Name => "Canceled";

        public override string GetPilotMessage() => "Flight is canceled.";
        public override int GetDelayMinutes() => 0;
    }
}
