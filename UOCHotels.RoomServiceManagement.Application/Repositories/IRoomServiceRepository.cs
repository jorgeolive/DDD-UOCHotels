using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Repositories
{
    public interface IRoomServiceRepository
    {
        Task<RoomServiceModel> GetById(Guid id);
        Task<List<RoomServiceModel>> GetByEmployeeId(Guid id);
        Task<List<RoomServiceModel>> GetByRoomId(Guid roomId);
        Task Commit();
        Task Add(RoomServiceModel roomService);
        Task<List<RoomServiceModel>> GetAll();
    }
}
