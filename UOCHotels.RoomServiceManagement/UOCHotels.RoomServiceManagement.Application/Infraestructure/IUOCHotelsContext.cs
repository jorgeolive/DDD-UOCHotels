using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UOCHotels.RoomServiceManagement.Domain;

namespace UOCHotels.RoomServiceManagement.Application.Infraestructure
{
    public interface IRoomServiceManagementContext
    {
        DbSet<RoomService> RoomServiceContext { get; set; }
        DbSet<Room> RoomContext { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
