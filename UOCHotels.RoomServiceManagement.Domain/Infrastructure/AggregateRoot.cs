using System;
using System.Collections.Generic;

namespace UOCHotels.RoomServiceManagement.Domain.Infrastructure
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<object> _events;

        protected AggregateRoot(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            _events = new List<object>();
        }

        protected abstract void When(object @event);

        public abstract void EnsureValidState();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _events.Add(@event);
        }
    }
}
