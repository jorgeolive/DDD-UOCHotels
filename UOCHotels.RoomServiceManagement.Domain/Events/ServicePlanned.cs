using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServicePlanned : INotification
    {
        public readonly Guid Id;
        public readonly DateTime PlannedOn;

        public ServicePlanned(Guid roomServiceId, DateTime plannedOn)
        {
            Id = roomServiceId;
            PlannedOn = plannedOn;
        }
    }
}
