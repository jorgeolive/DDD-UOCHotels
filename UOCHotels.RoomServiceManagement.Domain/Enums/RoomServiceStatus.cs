using System;
namespace UOCHotels.RoomServiceManagement.Domain.Enums
{
    public enum RoomServiceStatus
    {
        NotSet = 0,
        Inactive = 1,
        Planned = 2,
        Started = 3,
        Cancelled = 4,
        Completed = 5
    }
}
