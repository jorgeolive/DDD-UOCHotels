using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Address : ValueObject<Address>
    {
        public Building Building { get; internal set; }
        public DoorNumber DoorNumber { get; internal set; }
        public Floor Floor { get; internal set; }

        public static Address CreateFor(DoorNumber doorNumber, Floor roomFloor, Building buildingName)
        {
            return new Address(doorNumber, roomFloor, buildingName);
        }

        private Address(DoorNumber doorNumber, Floor roomFloor, Building building)
        {
            DoorNumber = doorNumber;
            Building = building;
            Floor = Floor;
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
