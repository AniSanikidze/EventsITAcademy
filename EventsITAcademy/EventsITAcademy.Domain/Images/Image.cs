using EventsITAcademy.Domain.Events;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsITAcademy.Domain.Images
{
    public class Image : BaseEntity
    {
        //private readonly IHostingEnvironment _webHost;
        //public Image(IHostingEnvironment webHost)
        //{
        //    _webHost = webHost;
        //}
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
                //var contentPath = _webHost.ContentRootPath;
                //var path = Path.Combine(contentPath, "Uploads");
                var fileWithPath = Path.Combine(path, ImageName);
                return fileWithPath;
                //string imageBase64Data = Convert.ToBase64String(ImageData);
                //string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                //return imageDataURL;
                //adaptedEvents[events.IndexOf(x)].ImageDataUrl = imageDataURL;
            }
        }
    }
}
