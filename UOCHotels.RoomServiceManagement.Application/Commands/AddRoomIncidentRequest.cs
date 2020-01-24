using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UOCHotels.RoomServiceManagement.Domain.Enums;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class AddRoomIncidentRequest : IRequest
    {
        public string Comment;
        public Guid RoomId;
        public Severity Severity;
        public Guid EmployeeId;
        public DateTime? CreatedOn;
    }
}
