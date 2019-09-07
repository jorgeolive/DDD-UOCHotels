using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class RoomId : ValueObject<RoomId>
    {
        public Guid Value { get; private set; }

        public RoomId() { }

        public RoomId(Guid id) =>
            Value = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => Value;

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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
