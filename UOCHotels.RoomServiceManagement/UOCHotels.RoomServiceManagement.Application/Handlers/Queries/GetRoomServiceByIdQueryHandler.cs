using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Queries
{
    public class GetRoomServiceByIdQueryHandler : IRequestHandler<GetRoomServiceByIdQuery, RoomServiceModel>
    {
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IRoomRepository _roomRepository;

        public GetRoomServiceByIdQueryHandler(IRoomServiceRepository roomServiceRepository, IRoomRepository roomRepository)
        {
            _roomServiceRepository = roomServiceRepository;
            _roomRepository = roomRepository;
        }

        public async Task<RoomServiceModel> Handle(GetRoomServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var roomService = await _roomServiceRepository.GetById(new RoomServiceId(request.RoomServiceId));

            if (roomService != null)
            {
                var room = await _roomRepository.GetById(roomService.AssociatedRoomId);

                return new RoomServiceModel()
                {
                    PlannedOn = roomService.PlannedOn,
                    Floor = room.Address.Floor.ToString(),
                    RoomNumber = room.Address.DoorNumber.ToString()
                };
            }

            throw new RoomServiceNotFoundException($"{request.RoomServiceId.ToString()}");
        }
    }
}
