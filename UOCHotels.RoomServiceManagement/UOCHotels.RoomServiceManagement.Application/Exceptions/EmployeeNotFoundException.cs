using System;
using System.Collections.Generic;
using System.Text;

namespace UOCHotels.RoomServiceManagement.Application.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message) : base(message)
        {
        }
    }
}
