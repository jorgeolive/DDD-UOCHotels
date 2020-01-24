using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class AddRoomIncidentPictureRequest : IRequest
    {
        public IFormFile File;
        public Guid RoomIncidentId;
    }
}
