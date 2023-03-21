using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Tickets;
using Microsoft.AspNetCore.Identity;

namespace EventsITAcademy.Domain.Users
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public EntityStatuses Status { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Event> Events { get; set; }
    }
}
