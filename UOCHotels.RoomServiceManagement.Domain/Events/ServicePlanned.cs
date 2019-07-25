using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class ServicePlanned : INotification
    {
        public Guid Id;
        public DateTime PlannedOn;
        public Guid EmployeeId;
        public Guid RoomId;
    }
}
