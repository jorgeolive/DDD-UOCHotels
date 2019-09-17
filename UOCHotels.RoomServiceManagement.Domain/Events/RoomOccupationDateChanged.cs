using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class RoomOccupationDateChanged : INotification
    {
        public Guid RoomId { get; private set; }
        public DateTime OccupationEndDate { get; private set; }

        public RoomOccupationDateChanged(Guid roomId, DateTime occupationEndDate)
        {
            RoomId = roomId;
            OccupationEndDate = occupationEndDate;
        }
    }
}
