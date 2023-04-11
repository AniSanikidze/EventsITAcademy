using EventsITAcademy.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Tickets.Requests
{
    public class TicketRequestModel
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        public int ReservationPeriodInMinutes { get; set; }
        public TicketStatuses TicketStatus { get; set; }
    }
}
