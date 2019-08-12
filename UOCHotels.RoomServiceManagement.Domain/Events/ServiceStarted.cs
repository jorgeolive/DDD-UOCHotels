using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceStarted : INotification
    {
        public Guid ServiceId { get; private set; }
        public DateTime StartTimestamp { get; private set; }

        public ServiceStarted(RoomServiceId roomServiceId, DateTime timestamp)
        {
            ServiceId = roomServiceId.GetValue();
            StartTimestamp = timestamp;
        }
    }
}
