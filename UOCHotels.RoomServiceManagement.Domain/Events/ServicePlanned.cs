using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServicePlanned : INotification
    {
        public Guid ServiceId { get; private set; }
        public DateTime PlannedOn { get; private set; }

        public ServicePlanned(Guid serviceId, DateTime plannedOn)
        {
            ServiceId = serviceId;
            PlannedOn = plannedOn;
        }
    }
}
