using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class EmployeeId : ValueObject<EmployeeId>
    {
        public Guid Value { get; private set; }

        protected EmployeeId() { }

        private EmployeeId(Guid id) => Value = id;

        public static EmployeeId CreateFor(Guid id) => id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : new EmployeeId(id);
        public Guid GetValue() => Value;

        protected override bool EqualsCore(EmployeeId other)
        {
            return this.Value == other.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
