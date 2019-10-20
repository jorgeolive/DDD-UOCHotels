using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class CreateRoomServiceRequest : IRequest
    {
        public CreateRoomServiceRequest(Guid roomId, Guid employeeId)
        {
            RoomId = roomId;
            EmployeeId = employeeId;
        }

        public Guid RoomId;
        public Guid EmployeeId;
    }
}
