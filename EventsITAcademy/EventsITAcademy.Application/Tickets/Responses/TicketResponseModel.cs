using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Tickets;
using EventsITAcademy.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Tickets.Responses
{
    public class TicketResponseModel
    {
        public string UserId { get; set; }
        public int EventId { get; set; }
        public TicketStatuses TicketStatus { get; set; }
        public DateTime ReservationDeadline { get; set; }
    }
}
