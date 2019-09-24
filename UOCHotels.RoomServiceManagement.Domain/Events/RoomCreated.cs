using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomCreated : INotification
    {
        public RoomCreated(Address address, RoomId roomId, RoomType roomType)
        {
            RoomId = roomId.Value;
            Floor = address.Floor.Value;
            DoorNumber = address.DoorNumber.Value;
            Building = address.Building.Name;
            RoomType = (int)roomType;
        }

        public Guid RoomId;
        public int Floor;
        public int DoorNumber;
        public string Building;
        public int RoomType;
    }
}
