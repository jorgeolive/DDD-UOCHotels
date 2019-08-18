using System;
namespace UOCHotels.RoomServiceManagement.Domain.SeedWork
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
}
