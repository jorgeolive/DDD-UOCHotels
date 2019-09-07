using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class EmployeeId : ValueObject<EmployeeId>
    {
        public Guid Value { get; private set; }

        protected EmployeeId() { }

        public EmployeeId(Guid id) =>
            Value = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => Value;

        protected override bool EqualsCore(EmployeeId other)
        {
            return this == other;
        }

        public static bool operator ==(EmployeeId id1, EmployeeId id2)
        {
            return id1.GetValue() == id2.GetValue();
        }

        public static bool operator !=(EmployeeId id1, EmployeeId id2)
        {
            return id1.GetValue() != id2.GetValue();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
