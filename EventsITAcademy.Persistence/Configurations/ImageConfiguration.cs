using EventsITAcademy.Domain.Images;
using EventsITAcademy.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Persistence.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.ModifiedAt).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.ImageName).IsRequired().HasColumnType("nvarchar(100)");

            builder.HasOne(x => x.Event).WithOne(x => x.Image).HasForeignKey<Image>(x => x.EventId);
        }
    }
}
