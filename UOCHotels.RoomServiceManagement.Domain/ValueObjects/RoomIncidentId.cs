using System;
using System.Collections.Generic;
using System.Text;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class RoomIncidentId : ValueObject<RoomIncidentId>
    {
        public Guid Value { get; private set; }

        protected RoomIncidentId() { }

        public static RoomIncidentId CreateFor(Guid id)
        {
            return new RoomIncidentId(id);
        }
        private RoomIncidentId(Guid id) =>
            Value = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => Value;

        protected override bool EqualsCore(RoomIncidentId other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
