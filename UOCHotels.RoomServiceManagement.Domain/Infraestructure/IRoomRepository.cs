using System;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Infraestructure
{
    public interface IRoomRepository
    {
        Task<Room> GetById(RoomId roomId);
    }
}
