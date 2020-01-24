using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class RoomServiceId : ValueObject<RoomServiceId>
    {
        public Guid Value { get; private set; }

        protected RoomServiceId() { }

        public static RoomServiceId CreateFor(Guid id)
        {
            return new RoomServiceId(id);
        }

        public RoomServiceId(Guid id) =>
            Value = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => Value;

        protected override bool EqualsCore(RoomServiceId other)
        {
            return this.Value == other.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
