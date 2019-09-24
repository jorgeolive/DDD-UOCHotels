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
    public class StartRoomServiceRequestHandler : AsyncRequestHandler<StartRoomServiceRequest>
    {
        private readonly IRoomServiceRepository _roomServiceRepository;
        private readonly IMediator mediator;

        public StartRoomServiceRequestHandler(IRoomServiceRepository context, IMediator mediator)
        {
            _roomServiceRepository = context;
            this.mediator = mediator;
        }

        protected override async Task Handle(StartRoomServiceRequest request, CancellationToken cancellationToken)
        {
            RoomService room = null;

            try
            {
                room = await this._roomServiceRepository.GetById(new RoomServiceId(request.RoomServiceId));
            }
            catch
            {
                throw new RoomServiceNotFoundException("");
            }

            room.Start();
            await _roomServiceRepository.Commit();
            //Here we might want to notify other services through the messaging queue. 
        }
    }
}
