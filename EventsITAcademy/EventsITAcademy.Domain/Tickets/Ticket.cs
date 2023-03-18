using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Domain.Tickets
{
    public class Ticket : BaseEntity
    {
        //public string UserId { get; set; }
        //public int EventId { get; set; }
        //public int TicketStatus { get; set; }
        ////public int ReservationPeriodInMinutes { get; set; }
        //public User User { get; set; }
        //public Event Event { get; set; }    
    }

    public enum TicketStatuses
    {
        Reserved,
        Bought
    }
}
