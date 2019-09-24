using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Infrastructure
{
    public interface IRoomRepository
    {
        Task<Room> GetById(RoomId roomId);
        Task<Room> GetByAddress(Address address);
        Task Commit();
        Task Add(Room room);
        Task<List<Room>> GetAll();
    }
}
