using System;
using Microsoft.EntityFrameworkCore;
using UOCHotels.RoomServiceManagement.Domain;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class RoomServiceManagementContext : DbContext
    {
        public RoomServiceManagementContext(DbContextOptions<RoomServiceManagementContext> options)
            : base(options) { }

        public DbSet<RoomService> RoomServices { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoomServiceManagementContext).Assembly);
        }

    }
}
