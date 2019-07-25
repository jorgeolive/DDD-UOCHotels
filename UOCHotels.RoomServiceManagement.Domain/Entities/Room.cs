using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Room : AggregateRoot
    {
        public DateTime AccomodationEndDate { get; internal set; }
        public Address Address;
        public List<RoomComplement> RoomComplements { get; internal set; }
        public List<RoomService> RoomServices { get; internal set; }
        public RoomType RoomType { get; internal set; }
        public bool ServicedToday => RoomServices.Any(x => x.EndTimeStamp?.Date == DateTime.UtcNow.Date);

        private Room() : base(Guid.NewGuid()) { }

        public Room(Address address) : base(Guid.NewGuid())
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            RoomComplements = new List<RoomComplement>();
        }

        public void AddComplement(RoomComplement roomComplement)
        {
            if (RoomComplements.Any(x => x == roomComplement)) throw new ComplementExistsException("");
            RoomComplements.Add(roomComplement);
        }

        public void AddService(RoomService service)
        {
            RoomServices.Add(service);
        }

        public void CompleteService(Guid roomServiceId, Guid employeeId)
        {
            if (!RoomServices.Any(x => x.Id == roomServiceId))
            {
                throw new ArgumentException(nameof(roomServiceId));
            }

            var roomService = RoomServices.Single(x => x.Id == roomServiceId);

            if (roomService.Status != RoomServiceStatus.Started)

                throw new InvalidRoomServiceOperationException("Room service is not started");


        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }

        public override void EnsureValidState()
        {
            throw new NotImplementedException();
        }
    }
}
