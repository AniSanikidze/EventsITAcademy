using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Images.Repositories;
using EventsITAcademy.Domain.Images;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //string uniqueFileName = null;
            //byte[]? bytes = null;
            //if(formFile != null)
            //{
            //    using (Stream fileStream = formFile.OpenReadStream())
            //    {
            //        using (BinaryReader binaryReader = new BinaryReader(fileStream))
            //        {
            //            bytes = binaryReader.ReadBytes((int)fileStream.Length);
            //        }
            //    }
            //}

            Image img = new Image();
            img.EventId = eventId;
            img.ImageName = uploadedImage.FileName;

            MemoryStream ms = new MemoryStream();
            uploadedImage.CopyTo(ms);
            img.ImageData = ms.ToArray();

            ms.Close();
            ms.Dispose();

            return await _imageRepository.SaveImageAsync(cancellationToken, img);
            //Image image = new Image()
            //{
            //    ImageData = Convert.ToBase64String(bytes, 0, bytes.Length),


            //};


        }
    }
}
