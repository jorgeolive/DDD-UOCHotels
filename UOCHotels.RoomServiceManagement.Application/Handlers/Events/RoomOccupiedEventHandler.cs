using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.IntegrationEvents;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Events
{
    public class RoomOccupationStartedHandler : INotificationHandler<RoomOccupationStarted>
    {
        readonly IRoomRepository _roomRepository;

        public RoomOccupationStartedHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task Handle(RoomOccupationStarted @event, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetByAddress(Address.CreateFor(DoorNumber.CreateFor(@event.RoomNumber), Floor.CreateFor(@event.Floor), Building.CreateFor(@event.Building)));
            if (room == null)
                return;
            //Not sure if this exception belongs to the application layer

            room.StartOccupation(@event.OccupationEndDate);
            await _roomRepository.Commit();
        }
    }
}
