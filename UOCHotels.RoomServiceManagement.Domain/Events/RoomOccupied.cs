using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomOccupied : INotification
    {
        public Guid RoomId { get; private set; }
        public DateTime OccupatiedOn { get; private set; }

        public RoomOccupied(Guid roomId, DateTime occupiedOn)
        {
            RoomId = roomId;
            OccupatiedOn = occupiedOn;
        }
    }
}
