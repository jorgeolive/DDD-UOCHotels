using System;
using System.Collections.Generic;

namespace UOCHotels.RoomServiceManagement.Domain.SeedWork
{
    public abstract class Entity
    {
        private readonly List<object> _events = new List<object>();

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