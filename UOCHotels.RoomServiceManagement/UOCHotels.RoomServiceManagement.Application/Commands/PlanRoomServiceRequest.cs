using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class PlanRoomServiceRequest : IRequest
    {
        public Guid RoomServiceId;
        public DateTime PlanDate;
        public Guid EmployeeId;

        public PlanRoomServiceRequest(Guid roomServiceId, DateTime planDate, Guid employeeId)
        {
            this.RoomServiceId = roomServiceId == default ? throw new ArgumentNullException(nameof(roomServiceId)) : roomServiceId;
            this.PlanDate = planDate == default ? throw new ArgumentNullException(nameof(planDate)) : planDate;
            this.EmployeeId = employeeId == default ? throw new ArgumentNullException(nameof(employeeId)) : employeeId;
        }
    }
}

