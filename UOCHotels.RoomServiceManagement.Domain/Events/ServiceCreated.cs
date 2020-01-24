using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceCreated : INotification
    {
        public DateTime CreatedOn { get; private set; }
        public Guid ServiceId { get; private set; }
        public Guid RoomId { get; private set; }
        public Guid EmployeeId { get; private set; }

        public ServiceCreated(DateTime createdOn, Guid serviceId, Guid roomId, Guid employeeId)
        {
            CreatedOn = createdOn;
            ServiceId = serviceId;
            RoomId = roomId;
            EmployeeId = employeeId;
        }
    }
}
