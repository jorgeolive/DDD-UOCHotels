using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class RoomServiceId : ValueObject<RoomServiceId>
    {
        public Guid Value { get; private set; }

        protected RoomServiceId() { }

        public RoomServiceId(Guid id) =>
            Value = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => Value;

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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
