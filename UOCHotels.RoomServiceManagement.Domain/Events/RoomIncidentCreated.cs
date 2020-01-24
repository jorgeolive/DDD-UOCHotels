using System;
using UOCHotels.RoomServiceManagement.Domain.Enums;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomIncidentCreated
    {
        public Guid RoomIncidentId { get; internal set; }
        public DateTime CreatedOn { get; internal set; }
        public string Comment { get; internal set; }
        public Guid EmployeeId { get; internal set; }
        public Guid RoomId { get; internal set; }
        public Severity Severity { get; internal set; }

        public RoomIncidentCreated(Guid roomIncidentId, Guid employeeId, Guid roomId, DateTime createdOn, string comment, Severity severity)
        {
            EmployeeId = employeeId;
            RoomIncidentId = roomIncidentId;
            CreatedOn = createdOn;
            Comment = comment;
            Severity = severity;
            RoomId = roomId;
        }
    }
}
