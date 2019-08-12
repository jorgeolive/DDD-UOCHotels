using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomOccupied : INotification
    {
        public Guid RoomId { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public RoomOccupied(RoomId roomId, DateTime timeStamp)
        {
            RoomId = roomId.GetValue();
            TimeStamp = timeStamp;
        }
    }
}
