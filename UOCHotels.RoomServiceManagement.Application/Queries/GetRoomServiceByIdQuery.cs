using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Queries
{
    public class GetRoomServiceByIdQuery : IRequest<RoomServiceModel>
    {
        public Guid RoomServiceId;

        public GetRoomServiceByIdQuery(Guid roomServiceId)
        {
            RoomServiceId = roomServiceId;
        }
    }
}
