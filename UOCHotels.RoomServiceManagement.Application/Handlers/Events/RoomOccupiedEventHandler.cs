using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.IntegrationEvents;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Events
{
    public class RoomOccupationStartedHandler : INotificationHandler<RoomOccupationStarted>
    {
        private readonly IRoomRepository roomRepository;        
        private readonly IAggregateStore store;
        public RoomOccupationStartedHandler(IAggregateStore store, IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
            this.store = store;
        }

        public async Task Handle(RoomOccupationStarted @event, CancellationToken cancellationToken)
        {

            var room = await store.Load<Room, RoomId>(RoomId.CreateFor(@event.RoomId)) as Room;
            room.StartOccupation(@event.OccupationEndDate);
            await store.Save<Room, RoomId>(room);
        }
    }
}
