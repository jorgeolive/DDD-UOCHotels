using System;
using System.Linq;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Extensions
{
    public static class RoomExtensions
    {
        public static RoomService GetRoomService(this Room room, RoomServiceId roomServiceId)
        {
            return room.RoomServices.Any(x => x.Id == roomServiceId) ? room.RoomServices.Single(x => x.Id == roomServiceId) : throw new ArgumentException(nameof(roomServiceId));
        }
    }
}
