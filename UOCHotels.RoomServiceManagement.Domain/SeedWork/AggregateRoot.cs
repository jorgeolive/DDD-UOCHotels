using System;
using System.Collections.Generic;

namespace UOCHotels.RoomServiceManagement.Domain.SeedWork
{
    public abstract class AggregateRoot<TId> where TId : ValueObject<TId>
    {
        public TId Id { get; protected set; }
        private readonly List<object> _changes;

        protected AggregateRoot()
        => _changes = new List<object>();


        protected abstract void When(object @event);

        public abstract void EnsureValidState();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }

        public void ClearChanges() => _changes.Clear();
    }
}
