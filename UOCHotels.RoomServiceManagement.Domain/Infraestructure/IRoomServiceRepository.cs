using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Infraestructure
{
    //Domain layer defines the IRepository objects.
    public interface IRoomServiceRepository
    {
        Task<RoomService> GetById(RoomServiceId id);
        Task<IEnumerable<RoomService>> GetByEmployeeId(EmployeeId id);
        Task<IEnumerable<RoomService>> GetByRoomId(RoomId roomId);
        Task Commit();
        Task Add(RoomService roomService);
    }
}
