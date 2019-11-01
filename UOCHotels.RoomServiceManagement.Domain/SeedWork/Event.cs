using System;
namespace UOCHotels.RoomServiceManagement.Domain.SeedWork
{
    public abstract class Event
    {
        public virtual string ToJson()
        {
            return string.Empty;
        }
    }
}
