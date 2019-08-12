using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceCreated : INotification
    {
        public Guid ServiceId { get; private set; }
        public Guid RoomId { get; private set; }
        public readonly DateTime PlannedOn;

        public ServiceCreated(RoomServiceId serviceId, DateTime plannedOn, EmployeeId workedById, RoomId roomId)
        {
            ServiceId = serviceId.GetValue();
            RoomId = roomId.GetValue();
            PlannedOn = plannedOn;
        }
    }
}
