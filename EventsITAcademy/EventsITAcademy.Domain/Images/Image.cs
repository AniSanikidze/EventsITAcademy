using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Domain.Images
{
    public class Image : BaseEntity
    {
        public string Url { get; set; }
        public string ImageName { get; set; }
    }
}
