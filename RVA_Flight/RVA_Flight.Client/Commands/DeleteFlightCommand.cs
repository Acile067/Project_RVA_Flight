using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Client.Commands
{
    public class DeleteFlightCommand:FlightCommand
    {
        public DeleteFlightCommand(ObservableCollection<Flight> flights, Flight flight)
        : base(flights, flight)
        { }

        public override void Execute()
        {
            flights.Remove(flight);
            ClientProxy.Instance.FlightService.DeleteFlight(flight);

        }

        public override void Undo()
        {
            flights.Add(flight);
            ClientProxy.Instance.FlightService.SaveFlight(flight);
        }
    }
}
