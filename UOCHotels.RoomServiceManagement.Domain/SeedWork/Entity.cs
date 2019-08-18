using System;
using System.Collections.Generic;

namespace UOCHotels.RoomServiceManagement.Domain.SeedWork
{
    //As per this implementation all events travel straight away to the aggregate root.
    //So this means, always attack point of Application Services is an AggregateRoot.
    public abstract class Entity<TId> : IInternalEventHandler
    {
        private readonly Action<object> _applier;
        public TId Id { get; protected set; }

        public Entity(Action<object> applier) => _applier = applier;

        protected abstract void When(object @event);

        public abstract void EnsureValidState();

        protected void Apply(object @event)
        {
            When(@event);
            _applier(@event);
        }

        public void Handle(object @event)
        => When(@event);
    }
}