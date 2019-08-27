using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Dto;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;

namespace UOCHotels.RoomServiceManagement.Application.Handlers
{
    public class GetRoomServicesByRoomIdQueryHandler : IRequestHandler<GetRoomServicesByRoomIdQuery, IEnumerable<RoomServiceDto>>
    {
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IRoomRepository _roomRepository;

        public GetRoomServicesByRoomIdQueryHandler(IRoomServiceRepository roomServiceRepository, IRoomRepository roomRepository)
        {
            _roomServiceRepository = roomServiceRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomServiceDto>> Handle(GetRoomServicesByRoomIdQuery request, CancellationToken cancellationToken)
        {
            var roomServicesDto = new List<RoomServiceDto>();
            var results = await _roomServiceRepository.GetByRoomId(new Domain.ValueObjects.RoomId(request.RoomId));

            if (results.Any())
            {

                foreach (var roomService in results)
                {
                    var room = await _roomRepository.GetById(roomService.AssociatedRoomId);

                    roomServicesDto.Add(
                        new RoomServiceDto()
                        {
                            PlannedOn = roomService.PlannedOn,
                            Floor = room.Address.Floor.ToString(),
                            RoomNumber = room.Address.DoorNumber.ToString()
                        }
                    );
                }
            }

            return roomServicesDto;
        }
    }
}