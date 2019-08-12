using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class RoomId : ValueObject<RoomId>
    {
        private Guid _id;

        public RoomId(Guid id) =>
            _id = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => _id;

        protected override bool EqualsCore(RoomId other)
        {
            return this == other;
        }

        public static bool operator ==(RoomId id1, RoomId id2)
        {
            return id1.GetValue() == id2.GetValue();
        }

        public static bool operator !=(RoomId id1, RoomId id2)
        {
            return id1.GetValue() != id2.GetValue();
        }
    }
}
