using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Client.Commands
{
    public abstract class FlightCommand:IFlightCommand
    {
        protected ObservableCollection<Flight> flights;
        protected Flight flight;

        protected FlightCommand(ObservableCollection<Flight> flights, Flight flight)
        {
            this.flights = flights;
            this.flight = flight;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}
