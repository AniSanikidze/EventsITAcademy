using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;

namespace EventsITAcademy.Domain.Tickets
{
    public class Ticket : BaseEntity
    {
        public string UserId { get; set; }
        public int EventId { get; set; }
        public TicketStatuses TicketStatus { get; set; }
        public DateTime? ReservationDeadline { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
    }

    public enum TicketStatuses
    {
        Reserved,
        Sold
    }
}
