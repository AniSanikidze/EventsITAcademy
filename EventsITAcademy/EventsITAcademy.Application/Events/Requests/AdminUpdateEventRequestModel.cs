using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Events.Requests
{
    public class AdminUpdateEventRequestModel
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
    }
}
