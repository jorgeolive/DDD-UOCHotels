using System;
using System.Threading.Tasks;

namespace UOCHotels.RoomServiceManagement.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
