using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomOccupied : INotification
    {
        public Guid RoomId { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public RoomOccupied(Guid id, DateTime timeStamp)
        {
            RoomId = id;
            TimeStamp = timeStamp;
        }
    }
}
