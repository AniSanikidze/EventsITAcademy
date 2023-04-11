using EventsITAcademy.Domain.Images;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Images
{
    public interface IImageService
    {
        Task<Image> SaveImageAsync(CancellationToken cancellationToken, IFormFile formFile, int eventId);
    }
}
