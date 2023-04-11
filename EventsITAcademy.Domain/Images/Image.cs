using EventsITAcademy.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsITAcademy.Domain.Images
{
    public class Image : BaseEntity
    {
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

        [NotMapped]
        public string ImageDataUrl
        {
            get
            {
                var path = "~/Uploads/";
                var fileWithPath = Path.Combine(path, ImageName);
                return fileWithPath;
            }
        }
    }
}
