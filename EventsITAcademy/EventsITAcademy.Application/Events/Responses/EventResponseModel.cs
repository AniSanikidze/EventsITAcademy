using EventsITAcademy.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool IsEditable { get; set; }
        public bool IsArchived { get; set; }
        public int ModificationPeriod { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
