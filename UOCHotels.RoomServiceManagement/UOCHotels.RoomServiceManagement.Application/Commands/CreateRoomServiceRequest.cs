using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class CreateRoomServiceRequest : IRequest
    {
        public Guid RoomId;

        public CreateRoomServiceRequest(Guid roomId) => this.RoomId = roomId == default ? throw new ArgumentNullException(nameof(roomId)) : roomId;
    }
}
