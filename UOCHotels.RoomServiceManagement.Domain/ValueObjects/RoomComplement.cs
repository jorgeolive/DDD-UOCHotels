using System;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class RoomComplement : ValueObject<RoomComplement>
    {
        public string Description { get; internal set; } 
        internal int RoomServiceEffortMinutes;

        public RoomComplement(string description, int roomServiceEffortMinutes)
        {
            Description = string.IsNullOrEmpty(description) ? throw new ArgumentNullException(nameof(description)) : description;
            RoomServiceEffortMinutes = (roomServiceEffortMinutes < 0) ? roomServiceEffortMinutes : throw new ArgumentOutOfRangeException(nameof(roomServiceEffortMinutes));
        }

        protected override bool EqualsCore(RoomComplement other)
        {
            return Description == other.Description;
        }
    }
}
