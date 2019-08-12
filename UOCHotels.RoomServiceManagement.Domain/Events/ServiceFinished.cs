using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceFinished : INotification
    {
        public Guid ServiceId { get; private set; }
        public DateTime Timestamp { get; internal set; }

        public ServiceFinished(RoomServiceId serviceId, DateTime closingTimestamp)
        {
            ServiceId = serviceId.GetValue();
            Timestamp = closingTimestamp;
        }
    }
}
