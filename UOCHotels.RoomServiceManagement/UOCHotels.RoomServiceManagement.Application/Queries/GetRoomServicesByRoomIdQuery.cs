using System;
using System.Collections.Generic;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Dto;

namespace UOCHotels.RoomServiceManagement.Application.Queries
{
    public class GetRoomServicesByRoomIdQuery : IRequest<IEnumerable<RoomServiceDto>>
    {
        public Guid RoomId;

        public GetRoomServicesByRoomIdQuery(Guid roomId)
        {
            RoomId = roomId;
        }
    }
}
