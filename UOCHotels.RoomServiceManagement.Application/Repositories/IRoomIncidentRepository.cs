using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Repositories
{
    public interface IRoomIncidentRepository
    {
        Task<RoomIncidentModel> GetById(Guid id);
        Task<List<RoomIncidentModel>> GetByRoomId(Guid id);
        Task Add(RoomIncidentModel room);
        Task Commit();
        Task<List<RoomIncidentModel>> GetAll();
    }
}
