using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class CreateRoomCommand : IRequest
    {
        public string BuildingName;
        public int RoomNumber;
        public int Floor;

        public CreateRoomCommand(string buildingName, int roomNumber, int floor)
        {
            BuildingName = buildingName;
            RoomNumber = roomNumber;
            Floor = floor;
        }
    }
}
