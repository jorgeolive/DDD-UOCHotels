using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Infraestructure
{
    public interface IRoomRepository
    {
        Task<Room> GetById(RoomId roomId);
        Task<Room> GetByAddress(Address address);
        Task Commit();
        Task Add(Room room);
    }
}
