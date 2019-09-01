using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class RoomServiceId : ValueObject<RoomServiceId>
    {
        private Guid _id;

        protected RoomServiceId() { }

        public RoomServiceId(Guid id) =>
            _id = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => _id;

        protected override bool EqualsCore(RoomServiceId other)
        {
            return this == other;
        }

        public static bool operator ==(RoomServiceId id1, RoomServiceId id2)
        {
            return id1.GetValue() == id2.GetValue();
        }

        public static bool operator !=(RoomServiceId id1, RoomServiceId id2)
        {
            return id1.GetValue() != id2.GetValue();
        }
    }
}
