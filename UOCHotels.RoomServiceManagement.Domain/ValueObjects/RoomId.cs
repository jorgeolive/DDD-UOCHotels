using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class RoomId : ValueObject<RoomId>
    {
        public Guid Value { get; private set; }

        public RoomId() { }

        public static RoomId CreateFor(Guid id)
        {
            return new RoomId(id);
        }
        public RoomId(Guid id) =>
            Value = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => Value;

        protected override bool EqualsCore(RoomId other)
        {
            return this.GetValue() == other.GetValue();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
