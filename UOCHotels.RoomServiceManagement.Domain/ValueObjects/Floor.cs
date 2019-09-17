using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class Floor : ValueObject<Floor>
    {
        public int Value { get; internal set; }

        public static Floor CreateFor(int floorNumber)
        {
            return new Floor(floorNumber);
        }

        protected Floor() { }

        private Floor(int floor)
        {
            Value = floor;
        }

        protected override bool EqualsCore(Floor other)
        {
            return Value == other.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
