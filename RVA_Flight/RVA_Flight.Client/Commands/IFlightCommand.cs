using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Client.Commands
{
    public interface IFlightCommand
    {
        void Execute();

        void Undo();
    }
}
