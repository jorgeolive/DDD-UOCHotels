using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class AddCommentRequest : IRequest
    {
        public string Text;
        public Guid RoomServiceId;
        public Guid EmployeeId;

        public AddCommentRequest(Guid roomServiceId, Guid employeeId, string text)
        {
            this.RoomServiceId = roomServiceId;
            this.Text = text;
            this.EmployeeId = employeeId;
        }
    }
}