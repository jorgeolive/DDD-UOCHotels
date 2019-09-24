using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
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
            var room = await _roomRepository.GetByAddress(Address.CreateFor(
                                  DoorNumber.CreateFor(request.RoomNumber),
                                  Floor.CreateFor(request.Floor),
                                  Building.CreateFor(request.BuildingName)));

            return new RoomModel()
            {
                RoomId = room.Id.GetValue(),
                Floor = room.Address.Floor.Value,
                Building = room.Address.Building.Name,
                Number = room.Address.DoorNumber.Value
            };
        }
    }
}
