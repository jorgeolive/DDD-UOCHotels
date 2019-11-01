using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class CreateRoomRequest : IRequest
    {
        public string BuildingName;
        public int RoomNumber;
        public int Floor;

        private CreateRoomRequest() { }

        public CreateRoomRequest(string buildingName, int roomNumber, int floor)
        {
            BuildingName = buildingName;
            RoomNumber = roomNumber;
            Floor = floor;
        }
    }
}
