using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceFinished : INotification
    {
        public Guid ServiceId { get; private set; }
        public DateTime Timestamp { get; internal set; }

        public ServiceFinished(Guid serviceId, DateTime closingTimestamp)
        {
            ServiceId = serviceId;
            Timestamp = closingTimestamp;
        }
    }
}
