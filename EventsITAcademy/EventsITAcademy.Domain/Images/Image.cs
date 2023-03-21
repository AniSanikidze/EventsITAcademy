using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Domain.Images
{
    public class Image : BaseEntity
    {
        public string ImageName { get; set; }
        public byte[] ImageData { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
