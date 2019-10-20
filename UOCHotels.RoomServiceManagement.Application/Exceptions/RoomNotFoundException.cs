using System;
namespace UOCHotels.RoomServiceManagement.Application.Exceptions
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException(string message) : base(message)
        {
        }
    }
}
