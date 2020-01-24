using System;
using System.Collections.Generic;
using System.Text;

namespace UOCHotels.RoomServiceManagement.Application.ReadModel
{
    public class RoomIncidentModel : IReadModel
    {
        public Guid Id;
        public Guid RoomId;
        public int RoomFloor;
        public int RoomNumber;
        public string Building;
        public List<string> Pictures;
        public DateTime CreatedOn;
        public string CreatedByFullName;
        public Guid EmployeeId;
        public string Comment;

        public string DbId => GetId();

        public string GetId()
        {
            return Id.ToString();
        }
    }
}
