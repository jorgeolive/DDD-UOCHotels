using System;
using System.Linq;
using UOCHotels.RoomServiceManagement.Domain.Enums;

namespace UOCHotels.RoomServiceManagement.Domain.DomainServices
{
    public class EstimateHouseKeepingService
    {
        public static int Calculate(Room room, Employee employee)
        {
            int estimate = 0;

            if (room.RoomType == RoomType.Simple) estimate += 20;
            if (room.RoomType == RoomType.Double) estimate += 30;
            if (room.RoomType == RoomType.Suite) estimate += 35;

            if (room.RoomComplements.Any())
            {
                foreach (var complement in room.RoomComplements)
                {
                    estimate += complement.RoomServiceEffortMinutes;
                }
            }

            if (employee.ExperienceMonths < 12) estimate += 25;

            if (room.OccupationEndDate.Date == DateTime.Now.Date) estimate += 25;

            return estimate;
        }
    }
}
