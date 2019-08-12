using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class EmployeeId : ValueObject<EmployeeId>
    {
        private Guid _id;

        public EmployeeId(Guid id) =>
            _id = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => _id;

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
    }
}
