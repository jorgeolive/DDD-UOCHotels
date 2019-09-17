using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.IntegrationEvents
{
    public class DailyRoomServicesCreated : INotification
    {
        public readonly bool AllRoomsCovered;
        public readonly int NumberOfServices;
        public IReadOnlyList<Guid> EmployeesNotAssigned;
        public readonly int HouseKeepersWithAssignment;

        public DailyRoomServicesCreated(
            bool allRoomsCovered,
            int numberOfServices,
            IEnumerable<Guid> employeesNotAssigned,
            int houseKeepersWithAssignment)
        {
            AllRoomsCovered = allRoomsCovered;
            NumberOfServices = numberOfServices;
            EmployeesNotAssigned = employeesNotAssigned.ToList();
            HouseKeepersWithAssignment = houseKeepersWithAssignment;
        }
    }
}
