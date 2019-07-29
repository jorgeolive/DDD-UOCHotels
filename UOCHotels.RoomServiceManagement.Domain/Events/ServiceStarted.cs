using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceStarted : INotification
    {
        public readonly Guid ServiceId;
        public readonly DateTime StartTimestamp;

        public ServiceStarted(Guid roomServiceId, DateTime timestamp)
        {
            ServiceId = roomServiceId;
            StartTimestamp = timestamp;
        }
    }
}
