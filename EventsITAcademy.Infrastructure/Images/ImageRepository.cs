using EventsITAcademy.Application.Images.Repositories;
using EventsITAcademy.Domain.Images;
using EventsITAcademy.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Infrastructure.Images
{
    public class ImageRepository : IImageRepository
    {
        readonly ApplicationContext _applicationContext;

        #region Ctor
        public ImageRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #endregion
        public async Task<Image> SaveImageAsync(CancellationToken cancellationToken, Image image)
        {
            await _applicationContext.Images.AddAsync(image, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return image;
        }

        
    }
}
