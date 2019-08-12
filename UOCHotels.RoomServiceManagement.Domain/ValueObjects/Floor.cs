using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Floor : ValueObject<Floor>
    {
        private int _floor;

        public static Floor CreateFor(int floorNumber)
        {
            return new Floor(floorNumber);
        }

        private Floor(int floor)
        {
            _floor = floor;
        }

        protected override bool EqualsCore(Floor other)
        {
            return _floor == other._floor;
        }

        public override string ToString()
        {
            return _floor.ToString();
        }
    }
}
