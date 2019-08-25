using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Business_Logic
{
    class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade, ILoginTokenBase
    {
        public void CancelTicket(LoginToken<Customer> token, Tickets ticket)
        {
            if(token!=null)
            {
                _ticketDAO.Remove(ticket);
            }
        }

        public List<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            List<Flight> flights = new List<Flight>();
            if (token != null)
            {
                flights = _flightDAO.GetAll();
            }
            return flights;
        }

        public Tickets PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            Tickets ticket = new Tickets();
            Flight newFlight = new Flight(flight.AIRLINECOMPANY_ID, flight.ORIGIN_COUNTRY_CODE, flight.DESTINATION_COUNTRY_CODE, flight.DEPARTURE_TIME, flight.LANDING_TIME, flight.REMAINING_TICKETS, flight.AIRLINECOMPANY_NAME,flight.FLIGHT_NAME);
            if (token != null)
            {
                if (newFlight.REMAINING_TICKETS>0)
                {
                    newFlight.REMAINING_TICKETS--;
                    _flightDAO.Update(newFlight);
                }
                ticket.CUSTOMER_ID = token.User.ID;
                ticket.FLIGHT_ID = flight.ID;
            }
            return ticket;
        }
    }
}
