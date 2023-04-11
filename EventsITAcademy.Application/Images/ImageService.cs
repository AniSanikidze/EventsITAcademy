using EventsITAcademy.Application.Images.Repositories;
using EventsITAcademy.Domain.Images;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace EventsITAcademy.Application.Images
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<Image> SaveImageAsync(CancellationToken cancellationToken, IFormFile uploadedImage, int eventId)
        {
            var contentPath = Path.GetFullPath("wwwroot");
            var path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var uniqueString = Guid.NewGuid().ToString();
            var newFileName = uniqueString + uploadedImage.FileName;
            string fileWithPath = Path.Combine(path, newFileName);
            var stream = new FileStream(fileWithPath, FileMode.Create);
       
                await uploadedImage.CopyToAsync(stream).ConfigureAwait(false);
                stream.Close();

            Image img = new Image();
            img.EventId = eventId;
            img.ImageName = newFileName;
            img.ImagePath = newFileName;

            MemoryStream ms = new MemoryStream();

            uploadedImage.CopyTo(ms);
            img.ImagePath = fileWithPath;

            ms.Close();
            ms.Dispose();

            return await _imageRepository.SaveImageAsync(cancellationToken, img);
        }
    }
}
