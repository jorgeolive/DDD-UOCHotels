using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Dto;

namespace UOCHotels.RoomServiceManagement.Application.Queries
{
    public class GetRoomServiceByIdQuery : IRequest<RoomServiceDto>
    {
        public Guid RoomServiceId;

        public GetRoomServiceByIdQuery(Guid roomServiceId)
        {
            RoomServiceId = roomServiceId;
        }
    }
}
