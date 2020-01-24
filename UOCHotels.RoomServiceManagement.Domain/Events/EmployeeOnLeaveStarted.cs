using MediatR;
using System;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class EmployeeOnLeaveStarted : INotification
    {
        public EmployeeOnLeaveStarted(Guid employeeId, DateTime? returnDate)
        {
            this.EmployeeId = employeeId;
            this.ReturnDate = returnDate;
        }

        public Guid EmployeeId { get; internal set; }
        public DateTime? ReturnDate { get; internal set; }
    }
}
