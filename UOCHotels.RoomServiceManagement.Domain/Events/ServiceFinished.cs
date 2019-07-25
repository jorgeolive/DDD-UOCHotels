using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceFinished : INotification
    {
        public Guid ServiceId { get; internal set; }
        public DateTime Timestamp { get; internal set; }

        public ServiceFinished(Guid serviceId, DateTime closingTimestamp)
        {
            ServiceId = serviceId;
            Timestamp = closingTimestamp;
        }
    }
}
