using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Queries
{
    public class GetRoomServicesByRoomIdQueryHandler : IRequestHandler<GetRoomServicesByRoomIdQuery, IEnumerable<RoomServiceModel>>
    {
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IRoomRepository _roomRepository;

        public GetRoomServicesByRoomIdQueryHandler(IRoomServiceRepository roomServiceRepository, IRoomRepository roomRepository)
        {
            _roomServiceRepository = roomServiceRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomServiceModel>> Handle(GetRoomServicesByRoomIdQuery request, CancellationToken cancellationToken)
        {
            var roomServicesModels = new List<RoomServiceModel>();
            var results = await _roomServiceRepository.GetByRoomId(request.RoomId);

            if (results.Any())
            {
                foreach (var roomService in results)
                {
                    var room = await _roomRepository.GetById(roomService.RoomId);

                    roomServicesModels.Add(
                        new RoomServiceModel()
                        {
                            PlannedOn = roomService.PlannedOn,
                            Floor = room.Floor,
                            RoomNumber = room.Number,
                            Building = room.Building,
                            Status = roomService.Status.ToString()
                        }
                    );
                }
            }

            return roomServicesModels;
        }
    }
}