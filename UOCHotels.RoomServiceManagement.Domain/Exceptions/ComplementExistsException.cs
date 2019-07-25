using System;
namespace UOCHotels.RoomServiceManagement.Domain.Exceptions
{
    public class ComplementExistsException : Exception
    {
        public ComplementExistsException(string message) : base(message)
        {            
        }
    }
}
