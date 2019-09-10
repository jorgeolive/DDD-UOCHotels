using System;
using System.Linq;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Services
{
    public class DailyServicePlanner
    {
        readonly IRoomRepository _roomRepository;
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IEmployeeRepository _employeeRepository;

        private const int ServicingStartsAt = 9;
        private const int RoomLeavesAt = 12;
        private const int dailyShift = 8 * 60;

        public DailyServicePlanner(
            IRoomRepository roomRepository,
            IEmployeeRepository employeeRepository,
            IRoomServiceRepository roomServiceRepository)
        {
            _employeeRepository = employeeRepository;
            _roomRepository = roomRepository;
            _roomServiceRepository = roomServiceRepository;

            //Get all rooms, identify these that need full Service. (Accomodation End Date = Next Day)
            //Get all employees
            //Implement an algorithm to get the most efficent distribution, considering factors such as
            // same floor, building , etc.
            // If the available employess are not enough push an event to the HotelManagement Bounded Context.  
        }

        public async Task Execute(Func<Room, Employee, bool, int> estimateDomainService)
        {
            var allRooms = await _roomRepository.GetAll();
            var allEmployees = await _employeeRepository.GetAll();

            //Gets the list of rooms per building and floor, ordered by these respectively.
            var groupedRoomsByFloorAndBuilding = allRooms.
                Where(x => (x.AccomodationEndDate.Date == DateTime.Today.Date) || (x.AccomodationEndDate.Date > DateTime.Today.Date && !x.ServicedToday)).
                GroupBy(x => new { building = x.Address.Building, floor = x.Address.Floor }).
                OrderBy(x => x.Key.building).ThenBy(x => x.Key.floor);

            var availableEmployees = allEmployees.Where(x => !x.OnLeave);
            var availableEmployeeCount = availableEmployees.Count();

            int index = 0;

            for (int i = 0; i < groupedRoomsByFloorAndBuilding.Count(); i++)
            {
                var group = groupedRoomsByFloorAndBuilding.ElementAt(i);
                var roomsByFloor = group.ToList();
                var employee = availableEmployees.ElementAt(index);
                int totalPendingWorkMinutes = 0;

                foreach (var room in roomsByFloor)
                {
                    if (totalPendingWorkMinutes < dailyShift)
                    {
                        totalPendingWorkMinutes += estimateDomainService(room, employee, room.AccomodationEndDate.Date == DateTime.Today.Date);
                        var roomService = RoomService.Create(new RoomServiceId(Guid.NewGuid()), room.Id);
                        roomService.Plan()
                    }
                    else
                    {
                        if (index < availableEmployeeCount - 1)
                        {
                            index++;
                            totalPendingWorkMinutes = 0;
                            employee = availableEmployees.ElementAt(index);
                        }

                        //stuff that could be done here.. raise event stating
                        //no enough employees to service
                    }
                }
            }
        }
    }
}
