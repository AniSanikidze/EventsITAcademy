using EventsITAcademy.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsITAcademy.Domain.Tickets;

namespace EventsITAcademy.Persistence.Configurations
{
    public class TicketConifguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            //builder.Property(x => x.Status).IsRequired();
            //builder.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime");
            //builder.Property(x => x.ModifiedAt).IsRequired().HasColumnType("datetime");

            //builder.HasOne(x => x.Event).WithMany(x => x.Tickets).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.NoAction);
            //builder.HasOne(x => x.User).WithMany(x => x.Tickets).HasForeignKey(x => x.UserId);



        }
    }
}
