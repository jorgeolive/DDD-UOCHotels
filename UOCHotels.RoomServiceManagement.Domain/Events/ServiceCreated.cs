using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServiceCreated : INotification
    {
        public Guid ServiceId { get; private set; }
        public Guid RoomId { get; private set; }
        public Guid EmployeeId { get; private set; }

        public ServiceCreated(RoomServiceId serviceId, RoomId roomId, EmployeeId employeeId)
        {
            ServiceId = serviceId.GetValue();
            RoomId = roomId.GetValue();
            EmployeeId = employeeId.GetValue();
        }
    }
}
