using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomLeft : INotification
    {
        //Domain Events doesn't include ValueObjects, such as RoomId. see pg 138
        //Events are inmutable, so properties should not be modificable.

        public Guid RoomId { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public RoomLeft(RoomId roomId, DateTime timeStamp)
        {
            RoomId = roomId.GetValue();
            TimeStamp = timeStamp;
        }
    }
}
