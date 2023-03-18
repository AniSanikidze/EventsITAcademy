using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Tickets;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Domain.Users
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public EntityStatuses Status { get; set; }
        //public List<Ticket> Tickets { get; set; }
        public List<Event> Events { get; set; }
    }
}
