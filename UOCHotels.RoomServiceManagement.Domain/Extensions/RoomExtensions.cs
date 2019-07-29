using System;
using System.Linq;

namespace UOCHotels.RoomServiceManagement.Domain.Extensions
{
    public static class RoomExtensions
    {
        public static RoomService GetRoomService(this Room room, Guid roomServiceId)
        {
            return room.RoomServices.Any(x => x.Id == roomServiceId) ? room.RoomServices.Single(x => x.Id == roomServiceId) : throw new ArgumentException(nameof(roomServiceId));
        }
    }
}
