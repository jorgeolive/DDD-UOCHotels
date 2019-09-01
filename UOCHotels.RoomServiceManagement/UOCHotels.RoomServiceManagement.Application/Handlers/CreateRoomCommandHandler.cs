using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers
{
    public class CreateRoomCommandHandler : AsyncRequestHandler<CreateRoomCommand>
    {
        readonly IMediator _mediator;
        readonly IRoomRepository _roomRepository;

        public CreateRoomCommandHandler(
                            IMediator mediator,
                            IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var requestedAddress = Address.CreateFor(
                                 DoorNumber.CreateFor(request.RoomNumber),
                                 Floor.CreateFor(request.Floor),
                                 Building.CreateFor(request.BuildingName));

            if (await _roomRepository.GetByAddress(requestedAddress) != null)
            {
                throw new InvalidOperationException($"Room with address {requestedAddress.ToString()} already exists in the system.");
            }

            //Refactor to factory method like RoomService class
            var room = new Room(requestedAddress);

            await _roomRepository.Add(room);

            foreach (var @event in room.GetChanges())
            {
                await _mediator.Publish(@event);
            }

            await _roomRepository.Commit();
        }
    }
}
