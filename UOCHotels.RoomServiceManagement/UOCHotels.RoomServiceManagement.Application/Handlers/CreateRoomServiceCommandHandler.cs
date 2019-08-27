using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.Interfaces;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers
{
    public class CreateRoomServiceCommandHandler : AsyncRequestHandler<CreateRoomServiceCommand>
    {
        readonly IMediator _mediator;
        readonly IRoomRepository _roomRepository;
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IUnitOfWork _unitOfWork;


        public CreateRoomServiceCommandHandler(
                            IMediator mediator,
                            IRoomRepository roomRepository,
                            IRoomServiceRepository roomServiceRepository,
                            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _roomServiceRepository = roomServiceRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        protected override async Task Handle(CreateRoomServiceCommand request, CancellationToken cancellationToken)
        {
            if (await _roomRepository.GetById(new Domain.ValueObjects.RoomId(request.RoomId)) == null)
                throw new RoomNotFoundException($"RoomId {request.RoomId.ToString()} does not exist.");

            var roomService = RoomService.Create(new RoomServiceId(Guid.NewGuid()), new RoomId(request.RoomId));

            foreach (var @event in roomService.GetChanges())
            {
                await _mediator.Publish(@event);
            }

            await _unitOfWork.Commit();
        }
    }
}
