using System;
namespace UOCHotels.RoomServiceManagement.Application.ReadModel
{
    public class RoomModel : IReadModel
    {
        public Guid Id;
        public string Building;
        public int Number;
        public int Floor;
        public DateTime OccupationEndDate;
        public DateTime OccupationStartDate;
        public bool Occupied;

        public string DbId => Id.ToString();

        public string GetId() => Id.ToString();
    }
}
