using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class EmployeeOnLeaveFinished : INotification
    {
        public EmployeeOnLeaveFinished(Guid employeeId, DateTime? returnDate)
        {
            this.EmployeeId = employeeId;
            this.ReturnDate = returnDate;
        }

        public Guid EmployeeId { get; internal set; }
        public DateTime? ReturnDate { get; internal set; }
    }
}
