using System;
using System.Linq;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Interfaces;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.DomainServices
{
    public class EstimateRoomServiceCalculator : IEstimateRoomServiceCalculator
    {
        public EstimateRoomServiceCalculator(IRoomRepository roomRepository, IEmployeeRepository employeeRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public int Calculate(Room room, Employee employee)
        {
            var room = await _roomRepository.GetById(roomId);
            var employee = await _roomRepository.GetById(roomId);

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
