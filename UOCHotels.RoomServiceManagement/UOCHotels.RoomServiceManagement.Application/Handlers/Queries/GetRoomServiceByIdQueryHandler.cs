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
        private readonly IRoomServiceRepository _roomServiceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEmployeeRepository _employeeRepository; 

        public GetRoomServiceByIdQueryHandler(IRoomServiceRepository roomServiceRepository, IRoomRepository roomRepository, IEmployeeRepository employeeRepository)
        {
            _roomServiceRepository = roomServiceRepository;
            _roomRepository = roomRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<RoomServiceModel> Handle(GetRoomServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var roomService = await _roomServiceRepository.GetById(new RoomServiceId(request.RoomServiceId));

            if (roomService != null)
            {
                var room = await _roomRepository.GetById(roomService.AssociatedRoomId);
                var employee = await _employeeRepository.GetById((roomService.ServicedById));

                return new RoomServiceModel()
                {
                    PlannedOn = roomService.PlannedOn,
                    Floor = room.Address.Floor.ToString(),
                    RoomNumber = room.Address.DoorNumber.ToString(),
                    Owner =  string.Concat(employee.Name," ",employee.SurName)
                };
            }

            throw new RoomServiceNotFoundException($"{request.RoomServiceId.ToString()}");
        }
    }
}
