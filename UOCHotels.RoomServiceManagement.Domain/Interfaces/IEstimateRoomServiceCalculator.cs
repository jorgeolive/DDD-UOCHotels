using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Interfaces
{
    public interface IEstimateRoomServiceCalculator
    {
        int Calculate(Room room, Employee employee);
    }
}
