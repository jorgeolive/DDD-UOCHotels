using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class CreateRoomServiceCommand : IRequest
    {
        public Guid RoomId;

        public CreateRoomServiceCommand(Guid roomId) => this.RoomId = roomId == default ? throw new ArgumentNullException(nameof(roomId)) : roomId;
    }
}
