using System;
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
    public class PlanRoomServiceRequestHandler : AsyncRequestHandler<PlanRoomServiceRequest>
    {
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IMediator _mediator;
        private readonly IAggregateStore _store;

        public PlanRoomServiceRequestHandler(
            IRoomServiceRepository roomServiceRepository,
            IMediator mediator,
            IAggregateStore store)
        {
            _roomServiceRepository = roomServiceRepository;
            _mediator = mediator;
            _store = store;
        }

        protected override async Task Handle(PlanRoomServiceRequest request, CancellationToken cancellationToken)
        {
            if (!(await this._roomServiceRepository.GetById(request.RoomServiceId) is RoomServiceModel model))
                throw new RoomServiceNotFoundException("");

            var roomAggregate = await _store.Load<RoomService, RoomServiceId>(RoomServiceId.CreateFor(request.RoomServiceId)) as RoomService;

            roomAggregate.Plan(request.PlanDate);

            await _store.Save<RoomService, RoomServiceId>(roomAggregate);
        }
    }
}
