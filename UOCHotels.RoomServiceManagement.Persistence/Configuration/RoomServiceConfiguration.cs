using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UOCHotels.RoomServiceManagement.Domain;

namespace UOCHotels.RoomServiceManagement.Persistence.Configuration
{
    public class RoomServiceConfiguration : IEntityTypeConfiguration<RoomService>
    {       
        public void Configure(EntityTypeBuilder<RoomService> builder)
        {
            builder.ToTable("RoomService");
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Room).WithMany(r => r.RoomServices);             
            builder.Property(e => e.EndTimeStamp).HasColumnType("ntext");
            builder.Property(e => e.Picture).HasColumnType("image");
            }
        }
    }
}
