using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Queries
{
    public class GetRoomByAddressQueryHandler : IRequestHandler<GetRoomByAddressQuery, RoomModel>
    {
        readonly IRoomRepository _roomRepository;

        public GetRoomByAddressQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomModel> Handle(GetRoomByAddressQuery request, CancellationToken cancellationToken)
        {
            return await _roomRepository.GetByAddress(request.BuildingName,request.RoomNumber,request.Floor);
        }
    }
}
