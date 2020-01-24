using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class CreateRoomServiceCommandHandler : AsyncRequestHandler<CreateRoomServiceRequest>
    {
        private readonly IMediator _mediator;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomServiceRepository _roomServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAggregateStore eventStore;

        public CreateRoomServiceCommandHandler(
                            IMediator mediator,
                            IRoomRepository roomRepository,
                            IRoomServiceRepository roomServiceRepository,
                            IEmployeeRepository employeeRepository,
                            IAggregateStore eventStore)
        {
            _roomRepository = roomRepository;
            _roomServiceRepository = roomServiceRepository;
            _employeeRepository = employeeRepository;
            _mediator = mediator;
            this.eventStore = eventStore;
        }

        protected override async Task Handle(CreateRoomServiceRequest request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetById(request.RoomId);
            if (room == null)
                throw new RoomNotFoundException($"RoomId {request.RoomId.ToString()} does not exist.");

            var employee = await _employeeRepository.GetById(request.EmployeeId);
            if (employee == null)
                throw new EmployeeNotFoundException($"Employee {request.EmployeeId.ToString()} does not exist.");

            var roomService = RoomService.CreateFor(RoomId.CreateFor(request.RoomId), EmployeeId.CreateFor(request.EmployeeId));

            await eventStore.Save<RoomService, RoomServiceId>(roomService);

            //We send the integration events after transaction is ok.
            foreach (var @event in roomService.GetChanges())
            {
                await _mediator.Publish(@event);
            }
        }
    }
}
