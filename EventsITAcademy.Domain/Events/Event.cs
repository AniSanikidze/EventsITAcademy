using EventsITAcademy.Domain.Images;
using EventsITAcademy.Domain.Tickets;
using EventsITAcademy.Domain.Users;

namespace EventsITAcademy.Domain.Events
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int NumberOfTickets { get; set; }    
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public int ModificationPeriod { get; set; }
        public int ReservationPeriod { get; set; }
        public string OwnerId { get; set; }
        public User User { get; set; }
        public List<Ticket> Tickets { get; set; }
        public Image Image { get; set; }
    }
}
