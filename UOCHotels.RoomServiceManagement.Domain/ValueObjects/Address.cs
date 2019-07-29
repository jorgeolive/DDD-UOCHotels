using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Address : ValueObject<Address>
    {
        public Building Building { get; internal set; }
        public string DoorNumber { get; internal set; }
        public Floor Floor { get; internal set; }

        public Address(string doorNumber, Floor roomFloor, Building building)
        {
            DoorNumber = !string.IsNullOrEmpty(doorNumber) ? doorNumber : throw new ArgumentNullException(nameof(doorNumber));
            Floor = roomFloor ?? throw new ArgumentNullException(nameof(roomFloor));
            Building = building ?? throw new ArgumentNullException(nameof(building));
        }

        public override string ToString()
        {
            return Building + "-" + Floor + "-" + DoorNumber;
        }

        protected override bool EqualsCore(Address other)
        {
            return other.ToString() == this.ToString();
        }
    }
}
