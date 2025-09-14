using RVA_Flight.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Client.Helpers
{
    public static class FlightTypeValues
    {
        public static Array All => Enum.GetValues(typeof(FlightType));
    }
}
