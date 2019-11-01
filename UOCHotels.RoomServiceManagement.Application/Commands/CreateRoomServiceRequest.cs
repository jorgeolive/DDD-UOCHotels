using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class CreateRoomServiceRequest : IRequest
    {
        public Guid RoomId;
        public Guid EmployeeId;

        public CreateRoomServiceRequest(Guid roomId, Guid employeeId)
        {
            RoomId = roomId;
            EmployeeId = employeeId;
        }

        private CreateRoomServiceRequest() { }
    }
}
