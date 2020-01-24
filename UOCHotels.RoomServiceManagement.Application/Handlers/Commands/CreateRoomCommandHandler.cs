using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;
using UOCHotels.RoomServiceManagement.Application.Exceptions;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class CreateRoomCommandHandler : AsyncRequestHandler<CreateRoomRequest>
    {
        readonly IMediator _mediator;
        readonly IRoomRepository _roomRepository;
        readonly IAggregateStore _eventStore;

        public CreateRoomCommandHandler(
                            IMediator mediator,
                            IRoomRepository roomRepository,
                            IAggregateStore eventStore)
        {
            _roomRepository = roomRepository;
            _mediator = mediator;
            _eventStore = eventStore;
        }

        protected override async Task Handle(CreateRoomRequest request, CancellationToken cancellationToken)
        {
            if ((await _roomRepository.GetByAddress(request.BuildingName, request.Floor, request.RoomNumber)) != null)
                throw new RoomExistsException("Room already exists.");

            var requestedAddress = Address.CreateFor(
                                 DoorNumber.CreateFor(request.RoomNumber),
                                 Floor.CreateFor(request.Floor),
                                 Building.CreateFor(request.BuildingName));

            var room = new Room(new RoomId(Guid.NewGuid()), RoomType.Other, requestedAddress);

            await _eventStore.Save<Room, RoomId>(room);

            var aggregate = await _eventStore.Load<Room, RoomId>(room.Id);
            
            foreach(var @event in room.GetChanges())            
                await _mediator.Publish(@event);
        }
    }
}
