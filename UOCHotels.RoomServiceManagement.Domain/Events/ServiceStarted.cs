using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceStarted : INotification
    {
        Guid ServiceId;
        DateTime StartTimestamp;

        public ServiceStarted(Guid roomServiceId, DateTime timestamp)
        {
            ServiceId = roomServiceId;
            StartTimestamp = timestamp;
        }
    }
}
