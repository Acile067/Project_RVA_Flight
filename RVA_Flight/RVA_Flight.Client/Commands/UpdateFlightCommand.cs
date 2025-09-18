using RVA_Flight.Client.Services;
using RVA_Flight.Common.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RVA_Flight.Client.Commands
{
    public class UpdateFlightCommand : FlightCommand
    {
        private Flight oldFlight;
        public UpdateFlightCommand(ObservableCollection<Flight> flights, Flight updatedFlight)
        : base(flights, updatedFlight)
        {
            var original = flights.FirstOrDefault(f => f.FlightNumber == updatedFlight.FlightNumber);
            if (original != null)
            {
                oldFlight = new Flight
                {
                    FlightNumber = original.FlightNumber,
                    Type = original.Type,
                    DepartureTime = original.DepartureTime,
                    ArrivalTime = original.ArrivalTime,
                    PilotMessage = original.PilotMessage,
                    DelayMinutes = original.DelayMinutes,
                    Airplane = original.Airplane,
                    Departure = original.Departure,
                    Arrival = original.Arrival,
                    State = original.State
                };
            }
        }

        
        public override void Execute()
        {
            var flightToUpdate = flights.FirstOrDefault(f => f.FlightNumber == flight.FlightNumber);
            if (flightToUpdate != null)
            {
                // update vrednosti
                flightToUpdate.Type = flight.Type;
                flightToUpdate.DepartureTime = flight.DepartureTime;
                flightToUpdate.ArrivalTime = flight.ArrivalTime;
                flightToUpdate.PilotMessage = flight.PilotMessage;
                flightToUpdate.DelayMinutes = flight.DelayMinutes;
                flightToUpdate.Airplane = flight.Airplane;
                flightToUpdate.Departure = flight.Departure;
                flightToUpdate.Arrival = flight.Arrival;
                flightToUpdate.State = flight.State;

            }

            ClientProxy.Instance.FlightService.DeleteFlight(oldFlight);
            
            ClientProxy.Instance.FlightService.SaveFlight(flight);
            

        }

        public override void Undo()
        {
            if (oldFlight == null) return;

            var flightToRevert = flights.FirstOrDefault(f => f.FlightNumber == oldFlight.FlightNumber);
            if (flightToRevert != null)
            {
                // vrati stare vrednosti
                flightToRevert.Type = oldFlight.Type;
                flightToRevert.DepartureTime = oldFlight.DepartureTime;
                flightToRevert.ArrivalTime = oldFlight.ArrivalTime;
                flightToRevert.PilotMessage = oldFlight.PilotMessage;
                flightToRevert.DelayMinutes = oldFlight.DelayMinutes;
                flightToRevert.Airplane = oldFlight.Airplane;
                flightToRevert.Departure = oldFlight.Departure;
                flightToRevert.Arrival = oldFlight.Arrival;
                flightToRevert.State = oldFlight.State;

                // update na server
                ClientProxy.Instance.FlightService.DeleteFlight(flight);
                ClientProxy.Instance.FlightService.SaveFlight(oldFlight);
            }


        }
    }
}
