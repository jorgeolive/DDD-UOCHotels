using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class StartRoomServiceRequest : IRequest
    {
        public Guid RoomServiceId;

        private StartRoomServiceRequest() { }

        public StartRoomServiceRequest(Guid roomServiceId)
        {
            if (roomServiceId == Guid.Empty)
                throw new ArgumentNullException(nameof(roomServiceId));
            RoomServiceId = roomServiceId;
        }
    }
}
