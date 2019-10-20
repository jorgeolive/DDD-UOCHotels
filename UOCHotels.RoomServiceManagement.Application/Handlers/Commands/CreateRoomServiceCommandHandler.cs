using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class CreateRoomServiceCommandHandler : AsyncRequestHandler<CreateRoomServiceRequest>
    {
        private readonly IMediator _mediator;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomServiceRepository _roomServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public CreateRoomServiceCommandHandler(
                            IMediator mediator,
                            IRoomRepository roomRepository,
                            IRoomServiceRepository roomServiceRepository,
                            IEmployeeRepository employeeRepository)
        {
            _roomRepository = roomRepository;
            _roomServiceRepository = roomServiceRepository;
            _employeeRepository = employeeRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(CreateRoomServiceRequest request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetById(new Domain.ValueObjects.RoomId(request.RoomId));
            if (room == null)
                throw new RoomNotFoundException($"RoomId {request.RoomId.ToString()} does not exist.");

            var employee = await _employeeRepository.GetById(new Domain.ValueObjects.EmployeeId(request.EmployeeId));
            if (employee == null)
                throw new EmployeeNotFoundException($"Employee {request.EmployeeId.ToString()} does not exist.");

            var roomService = new RoomService(new RoomServiceId(Guid.NewGuid()), new RoomId(request.RoomId), new EmployeeId(request.EmployeeId));

            await _roomServiceRepository.Add(roomService);
            await _roomServiceRepository.Commit();

            //We send the integration events after transaction is ok.
            foreach (var @event in roomService.GetChanges())
            {
                await _mediator.Publish(@event);
            }
        }
    }
}
