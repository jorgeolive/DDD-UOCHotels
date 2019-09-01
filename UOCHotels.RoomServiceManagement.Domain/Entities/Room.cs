using System;
using System.Collections.Generic;
using System.Linq;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Extensions;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Room : AggregateRoot<RoomId>
    {
        public DateTime AccomodationEndDate { get; internal set; }
        public Address Address;
        public bool BeingServiced { get; internal set; }
        public List<RoomComplement> RoomComplements { get; internal set; }
        public List<RoomService> RoomServices { get; internal set; }
        public RoomType RoomType { get; internal set; }
        public bool ServicedToday => RoomServices.Any(x => x.EndTimeStamp?.Date == DateTime.UtcNow.Date);

        private string DbId
        {
            get => $"Room/{Id.ToString()}";
            set { }
        }

        public Room(Address address)
        {
            Id = new RoomId(Guid.NewGuid());
            Address = address ?? throw new ArgumentNullException(nameof(address));
            RoomComplements = new List<RoomComplement>();
            RoomServices = new List<RoomService>();
        }

        public void AddComplement(RoomComplement roomComplement)
        {
            if (RoomComplements.Any(x => x == roomComplement))
            {
                throw new ComplementExistsException(roomComplement.ToString());
            }

            RoomComplements.Add(roomComplement);
        }

        public void Service() => this.BeingServiced = true;

        public void EndService() => this.BeingServiced = false;

        public override void EnsureValidState()
        {
            throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}
