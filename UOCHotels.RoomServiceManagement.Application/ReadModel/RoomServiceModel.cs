using System;
namespace UOCHotels.RoomServiceManagement.Application.ReadModel
{
    public class RoomServiceModel : IReadModel
    {
        public Guid Id { get; set; }
        public DateTime? PlannedOn { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public int RoomNumber { get; set; }
        public string Building { get; set; }
        public int Floor { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid RoomId { get; set; }

        public string DbId => Id.ToString();

        public string GetId()
        {
            return Id.ToString();
        }
    }
}
