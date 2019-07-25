using System;
namespace UOCHotels.RoomServiceManagement.Domain.Exceptions
{
    public class InvalidRoomServiceOperationException: Exception {

        public InvalidRoomServiceOperationException(string message) : base(message)
        {
        }
    }
}
