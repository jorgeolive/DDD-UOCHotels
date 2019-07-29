using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceCreated : INotification
    {
        public readonly Guid Id, EmployeeId, RoomId;
        public readonly DateTime PlannedOn;

        public ServiceCreated(Guid serviceId, DateTime plannedOn, Guid workedById, Guid roomId)
        {
            Id = serviceId;
            RoomId = roomId;
            EmployeeId = workedById;
            PlannedOn = plannedOn;
        }
    }
}
