using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class StartRoomServiceRequestHandler : AsyncRequestHandler<StartRoomServiceRequest>
    {
        private readonly IRoomServiceRepository _roomServiceRepository;
        private readonly IAggregateStore _store;

        public StartRoomServiceRequestHandler(IRoomServiceRepository context, IAggregateStore store)
        {
            _roomServiceRepository = context;
            _store = store;
        }

        protected override async Task Handle(StartRoomServiceRequest request, CancellationToken cancellationToken)
        {
            if (!(await this._roomServiceRepository.GetById(request.RoomServiceId) is RoomServiceModel model))
                throw new RoomServiceNotFoundException("");

            var roomAggregate = await _store.Load<RoomService, RoomServiceId>(RoomServiceId.CreateFor(request.RoomServiceId)) as RoomService;

            roomAggregate.Start();

            await _store.Save<RoomService, RoomServiceId>(roomAggregate);
        }
    }
}
