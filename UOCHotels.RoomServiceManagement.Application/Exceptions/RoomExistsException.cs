using System;
using System.Collections.Generic;
using System.Text;

namespace UOCHotels.RoomServiceManagement.Application.Exceptions
{
    public class RoomExistsException : Exception
    {
        public RoomExistsException(string message) : base(message)
        {
        }
    }
}
