using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class StartRoomServiceCommand : IRequest
    {
        public Guid RoomId;
        public Guid RoomServiceId;
    }
}
