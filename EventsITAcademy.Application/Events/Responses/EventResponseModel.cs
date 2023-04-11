namespace EventsITAcademy.Application.Events.Responses
{
    public class EventResponseModel
    {
        public int Id { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public DateTime ModificationDeadline => CreatedAt.AddDays(ModificationPeriod);
        public string ImageDataUrl { get; set; }
    }
}
