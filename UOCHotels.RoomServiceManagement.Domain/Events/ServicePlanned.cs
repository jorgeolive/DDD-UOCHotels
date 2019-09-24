using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServicePlanned : INotification
    {
        public Guid Id { get; private set; }
        public DateTime PlannedOn { get; private set; }

        public ServicePlanned(RoomServiceId roomServiceId, DateTime plannedOn)
        {
            Id = roomServiceId.GetValue();
            PlannedOn = plannedOn;
        }
    }
}
