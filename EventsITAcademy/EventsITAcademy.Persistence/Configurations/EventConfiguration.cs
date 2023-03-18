using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsITAcademy.Domain.Events;

namespace EventsITAcademy.Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(350);
            builder.Property(x => x.StartDate).IsRequired().HasColumnType("date");
            builder.Property(x => x.FinishDate).IsRequired().HasColumnType("date");
            builder.Property(x => x.NumberOfTickets).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.ModificationPeriod).IsRequired();

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.ModifiedAt).IsRequired().HasColumnType("datetime");

            builder.HasOne(x => x.User).WithMany(x => x.Events).HasForeignKey(x => x.OwnerId);
        }
    }
}
