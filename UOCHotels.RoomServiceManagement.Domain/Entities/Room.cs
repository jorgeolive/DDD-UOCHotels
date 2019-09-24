using System;
using System.Collections.Generic;
using System.Linq;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Entities
{
    public class Room : AggregateRoot<RoomId>
    {
        public DateTime OccupationEndDate { get; internal set; }

        public Address Address;
        public bool BeingServiced { get; private set; }
        public List<RoomComplement> RoomComplements { get; private set; }
        public List<RoomServiceId> RoomServices { get; private set; }
        public RoomType RoomType { get; private set; }
        public bool ServicedToday  { get; private set; }
    public bool IsActive { get; internal set; } = true;

        //This is due to database impedance. RavenDb needs this. 
        private string DbId
        {
            get => $"Room/{Id.ToString()}";
            set { }
        }

        //This is for RavenDb
        protected Room() { }

        public Room(RoomId roomId, RoomType roomType, Address address)
        {
            if (roomId.Value == Guid.Empty) throw new ArgumentNullException();
            Apply(new RoomCreated(address, roomId, roomType));
        }

        public void AddComplement(RoomComplement roomComplement)
        {
            if (RoomComplements.Any(x => x == roomComplement))
            {
                throw new ComplementExistsException(roomComplement.ToString());
            }

            RoomComplements.Add(roomComplement);
        }

        public static Room Create(RoomId roomId, RoomType roomType, Address address)
        {
            if (roomId.Value == Guid.Empty) throw new ArgumentNullException();
            return new Room(roomId, roomType, address);
        }

        public void StartOccupation(DateTime endDate)
        {
            if (endDate.Date < DateTime.Now.Date)
                throw new ArgumentOutOfRangeException(nameof(endDate));

            Apply(new RoomOccupationDateChanged(this.Id.Value, endDate));
        }

        public void Service() => this.BeingServiced = true;

        public void EndService() => this.BeingServiced = false;

        public override void EnsureValidState()
        {
            //throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case RoomOccupationDateChanged roomOccupied:
                    {
                        this.OccupationEndDate = roomOccupied.OccupationEndDate;
                    }

                    break;

                case RoomCreated roomCreated:
                    {
                        this.Id = new RoomId(roomCreated.RoomId);
                        this.RoomType = (RoomType)roomCreated.RoomType;
                        this.Address = Address.CreateFor(DoorNumber.CreateFor(roomCreated.DoorNumber),
                            Floor.CreateFor(roomCreated.Floor), Building.CreateFor(roomCreated.Building));
                    }
                    break;
            }
        }
    }
}
