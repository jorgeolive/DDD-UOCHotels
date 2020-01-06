using System;
using System.Collections.Generic;
using System.Linq;

namespace UOCHotels.RoomServiceManagement.Domain.SeedWork
{
    public abstract class AggregateRoot<TId> : IInternalEventHandler where TId : ValueObject<TId>
    {
        public long Version = 0;
        public TId Id { get; protected set; }
        private readonly List<object> _changes;

        protected AggregateRoot()
        => _changes = new List<object>();

        protected abstract void When(object @event);

        public abstract void EnsureValidState();

        public void Load(IEnumerable<object> history)
        {
            foreach (var e in history)
            {
                When(e);
                Version++;
            }
        }

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }

        //Need to understand this concept a bit better.
        protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);

        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

        public void ClearChanges() => _changes.Clear();

        public void Handle(object @event) => When(@event);
    }
}
