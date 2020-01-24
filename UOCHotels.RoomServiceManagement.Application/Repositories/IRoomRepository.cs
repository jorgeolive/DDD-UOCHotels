using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Repositories
{
    public interface IRoomRepository
    {
        Task<RoomModel> GetById(Guid id);
        Task<RoomModel> GetByAddress(string building, int floor, int number);
        Task Add(RoomModel room);
        Task Commit();
        Task<List<RoomModel>> GetAll();
    }
}
