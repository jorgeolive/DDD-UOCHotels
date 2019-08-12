using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServicePlanned : INotification
    {
        public Guid Id { get; private set; }
        public DateTime PlannedOn { get; private set; }
        public Guid ServiceOwnerId { get; private set; }

        public ServicePlanned(RoomServiceId roomId, EmployeeId ownerId, DateTime plannedOn)
        {
            Id = roomId.GetValue();
            PlannedOn = plannedOn;
            ServiceOwnerId = ownerId.GetValue();
        }
    }
}
