//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using UOCHotels.RoomServiceManagement.Domain;
//using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

//namespace UOCHotels.RoomServiceManagement.Persistence.Configuration
//{
//    public class RoomServiceConfiguration : IEntityTypeConfiguration<RoomService>
//    {
//        public void Configure(EntityTypeBuilder<RoomService> builder)
//        {
//            builder.ToTable("RoomService");
//            builder.OwnsOne(e => e.Id);
//            builder.OwnsOne(e => e.AssociatedRoomId);
//            builder.OwnsOne(e => e.ServicedById);
//            builder.HasKey(e => e.Id);
//            builder.Property(e => e.EndTimeStamp).HasColumnType("ntext");
//            builder.Property(e => e.StartTimeStamp).HasColumnType("ntext");
//            builder.Property(e => e.PlannedOn).HasColumnType("ntext");
//            builder.Property(e => e.Status).HasColumnType("ntext");

//            builder.OwnsMany<Comment>("Comments", a =>
//             {
//                 a.HasForeignKey("RoomServiceId");
//                 a.Property(ca => ca.CommentBy);
//                 a.Property(ca => ca.Text);
//                 a.Property(ca => ca.CreatedOn);
//             });
//        }
//    }
//}

