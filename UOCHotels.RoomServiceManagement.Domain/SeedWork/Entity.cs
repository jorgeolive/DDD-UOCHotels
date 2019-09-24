using System;
using System.Collections.Generic;

namespace UOCHotels.RoomServiceManagement.Domain.SeedWork
{
    //As per this implementation all events travel straight away to the aggregate root.
    //So this means, always attack point of Application Services is an AggregateRoot.
    public abstract class Entity<TId> : IInternalEventHandler
    {
        protected readonly Action<object> Applier;
        public TId Id { get; protected set; }

        protected Entity(Action<object> applier) => Applier = applier;

        protected abstract void When(object @event);

        public abstract void EnsureValidState();

        protected void Apply(object @event)
        {
            When(@event);
            Applier(@event);
        }

        public void Handle(object @event)
        => When(@event);
    }
}