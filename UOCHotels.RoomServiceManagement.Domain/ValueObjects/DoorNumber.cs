using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{

    public class DoorNumber : ValueObject<DoorNumber>
    {
        public int Value { get; internal set; }

        public static DoorNumber CreateFor(int doorNumber)
        {
            return new DoorNumber(doorNumber);
        }

        //For serialization.
        protected DoorNumber() { }

        private DoorNumber(int doorNumber)
        {
            Value = doorNumber;
        }

        protected override bool EqualsCore(DoorNumber other)
        {
            return Value == other.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}