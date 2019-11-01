using System;
namespace UOCHotels.RoomServiceManagement.Application.ReadModel
{
    public class RoomServiceModel
    {
        public DateTime? PlannedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string RoomNumber { get; set; }
        public string Floor { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
    }
}
