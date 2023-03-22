using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public string ImageDataUrl
        {
            get
            {
                string imageBase64Data = Convert.ToBase64String(ImageData);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                return imageDataURL;
                //adaptedEvents[events.IndexOf(x)].ImageDataUrl = imageDataURL;
            }
        }
    }
}
