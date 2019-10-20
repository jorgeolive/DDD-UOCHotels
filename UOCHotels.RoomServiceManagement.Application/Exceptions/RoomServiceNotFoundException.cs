using System;
namespace UOCHotels.RoomServiceManagement.Application.Exceptions
{
    public class RoomServiceNotFoundException : Exception
    {
        public RoomServiceNotFoundException(string message) : base(message)
        {
        }
    }
}
