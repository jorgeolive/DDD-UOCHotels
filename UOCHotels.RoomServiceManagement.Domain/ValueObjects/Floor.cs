using System;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Floor : ValueObject<Floor>
    {
        private int _floor;

        public Floor(int floor)
        {
            if (floor < 0) throw new ArgumentException("The floor value is not valid");
            _floor = floor;
        }

        protected override bool EqualsCore(Floor other)
        {
            return _floor == other._floor;
        }
    }
}
