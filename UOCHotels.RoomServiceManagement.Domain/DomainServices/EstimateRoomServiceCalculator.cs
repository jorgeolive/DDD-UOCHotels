using System;
using System.Linq;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Interfaces;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.DomainServices
{
    public class EstimateRoomServiceCalculator : IEstimateRoomServiceCalculator
    {
        public int Calculate(Room room, Employee employee)
        {
            int estimate = 0;

            if (room.RoomType == RoomType.Simple) estimate += 45;
            if (room.RoomType == RoomType.Double) estimate += 60;
            if (room.RoomType == RoomType.Suite) estimate += 75;

            if (room.RoomComplements.Any())
            {
                foreach (var complement in room.RoomComplements)
                {
                    estimate += complement.RoomServiceEffortMinutes;
                }
            }

            if (employee.ExperienceMonths < 12) estimate += 25;

            return estimate;
        }
    }
}
