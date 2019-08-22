using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers
{
    public class CreateRoomServiceCommandHandler : AsyncRequestHandler<CreateRoomServiceCommand>
    {
        readonly IMediator _mediator;
        readonly IRoomRepository _roomRepository;

        public CreateRoomServiceCommandHandler(IMediator mediator, IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(CreateRoomServiceCommand request, CancellationToken cancellationToken)
        {
            if (await _roomRepository.GetById(new Domain.ValueObjects.RoomId(request.RoomId)) == null)
                throw new InvalidOperationException("RoomId does not exist.");

            RoomService.Create(new RoomServiceId(Guid.NewGuid()), new RoomId(request.RoomId));
        }
    }
}
