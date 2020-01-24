using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomCreated : INotification
    {
        public RoomCreated(Guid roomId, int floor, int doorNumber, string building, int roomType)
        {
            RoomId = roomId;
            Floor = floor;
            DoorNumber = doorNumber;
            Building = building;
            RoomType = roomType;
        }

        //Events are inmutable, so ... no setters.
        public Guid RoomId { get; }
        public int Floor { get; }
        public int DoorNumber { get; }
        public string Building { get; }
        public int RoomType { get; }
    }
}
