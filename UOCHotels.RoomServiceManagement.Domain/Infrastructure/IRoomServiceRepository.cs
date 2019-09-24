using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Infrastructure
{
    //Domain layer defines the IRepository objects.
    public interface IRoomServiceRepository
    {
        Task<RoomService> GetById(RoomServiceId id);
        Task<List<RoomService>> GetByEmployeeId(EmployeeId id);
        Task<List<RoomService>> GetByRoomId(RoomId roomId);
        Task Commit();
        Task Add(RoomService roomService);
        Task<List<RoomService>> GetAll();
    }
}
