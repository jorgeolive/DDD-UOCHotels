using System;
using System.Collections.Generic;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Queries
{
    public class GetRoomServicesByRoomIdQuery : IRequest<IEnumerable<RoomServiceModel>>
    {
        public Guid RoomId;

        public GetRoomServicesByRoomIdQuery(Guid roomId)
        {
            RoomId = roomId;
        }
    }
}
