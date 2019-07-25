using System;
namespace UOCHotels.RoomServiceManagement.Domain.Interfaces
{
    public interface IEstimateRoomServiceCalculator
    {
        int Calculate(Room room, Employee employee);
    }
}
