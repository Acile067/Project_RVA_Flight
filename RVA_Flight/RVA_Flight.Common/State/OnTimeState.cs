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

        public override string GetPilotMessage() => "Flight is on time.";
        public override int GetDelayMinutes() => 0;
    }
}
