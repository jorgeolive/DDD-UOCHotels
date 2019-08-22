using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class CreateRoomServiceCommand : IRequest
    {
        public Guid RoomId;

        public CreateRoomServiceCommand()
        {
        }
    }
}
