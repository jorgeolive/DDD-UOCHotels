using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomLeft : INotification
    {
        public Guid RoomId { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public RoomLeft(Guid id, DateTime timeStamp)
        {
            RoomId = id;
            TimeStamp = timeStamp;
        }
    }
}
