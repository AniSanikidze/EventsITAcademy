using EventsITAcademy.Domain.Images;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Images.Repositories
{
    public interface IImageRepository
    {
        Task<Image> SaveImageAsync(CancellationToken cancellationToken, Image image);
        //Task<Image> GetImageAsync(CancellationToken cancellationToken,int eventId);
    }
}
