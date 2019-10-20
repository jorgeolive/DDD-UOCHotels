using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class StartRoomServiceRequest : IRequest
    {
        public Guid RoomServiceId;
    }
}
