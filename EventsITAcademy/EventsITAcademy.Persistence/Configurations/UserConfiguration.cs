using EventsITAcademy.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasIndex(x => x.UserName).IsUnique();
            //builder.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            //builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Status).IsRequired();
            //builder.Property(x => x.CreatedAt).IsRequired().HasColumnType("datetime");
            //builder.Property(x => x.ModifiedAt).IsRequired().HasColumnType("datetime");
        }
    }
}
