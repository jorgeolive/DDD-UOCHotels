using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.DomainServices;
using UOCHotels.RoomServiceManagement.Application.IntegrationEvents;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Services
{
    public class HouseKeepingPlanner
    {
        readonly IRoomRepository _roomRepository;
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IEmployeeRepository _employeeRepository;
        readonly IMediator _mediator;

        private const int ServicingStartsAt = 9;
        private const int dailyShift = 8 * 60;

        private int _createdServices = 0;
        private int _totalHouseKeepers;
        private int _houseKeepersWithAssignment;

        public HouseKeepingPlanner(
            IMediator mediator,
            IRoomRepository roomRepository,
            IEmployeeRepository employeeRepository,
            IRoomServiceRepository roomServiceRepository
            )
        {
            _mediator = mediator;
            _employeeRepository = employeeRepository;
            _roomRepository = roomRepository;
            _roomServiceRepository = roomServiceRepository;

            //Get all rooms, identify these that need full Service. (Accomodation End Date = Next Day)
            //Get all employees
            //Implement an algorithm to get the most efficent distribution, considering factors such as
            // same floor, building , etc.
            // If the available employess are not enough push an event to the HotelManagement Bounded Context.  
        }

        public async Task Execute(object state)
        {
            var allRooms = await _roomRepository.GetAll();
            var allEmployees = await _employeeRepository.GetAll();

            //Gets the list of rooms per building and floor, ordered by these respectively.
            var groupedRoomsByFloorAndBuilding = allRooms.
                Where(x => (x.OccupationEndDate.Date == DateTime.Today.Date) || (x.OccupationEndDate.Date > DateTime.Today.Date && !x.ServicedToday)).
                GroupBy(x => new { building = x.Address.Building, floor = x.Address.Floor }).
                OrderBy(x => x.Key.building).ThenBy(x => x.Key.floor);

            var availableEmployees = allEmployees.Where(x => !x.OnLeave);
            _totalHouseKeepers = availableEmployees.Count();

            int index = 0;
            _houseKeepersWithAssignment = index + 1;

            for (int i = 0; i < groupedRoomsByFloorAndBuilding.Count(); i++)
            {
                var group = groupedRoomsByFloorAndBuilding.ElementAt(i);
                var roomsByFloor = group.ToList();
                var employee = availableEmployees.ElementAt(index);
                int assignedMinutes = 0;
                DateTime start = DateTime.Now.Date.AddHours(ServicingStartsAt);

                foreach (var room in roomsByFloor)
                {
                    if (assignedMinutes < dailyShift)
                    {
                        var serviceEstimation = EstimateHouseKeepingService.Calculate(room, employee);
                        assignedMinutes += serviceEstimation;

                        var roomService = new RoomService(new RoomServiceId(Guid.NewGuid()), room.Id, employee.Id);
                        _createdServices++;
                        roomService.Plan(start.AddMinutes(assignedMinutes));
                        await _roomServiceRepository.Add(roomService);
                    }
                    else
                    {
                        if (index < _totalHouseKeepers - 1)
                        {
                            index++;
                            assignedMinutes = 0;
                            employee = availableEmployees.ElementAt(index);
                        }
                        else
                        {
                            await _roomServiceRepository.Commit();

                            await _mediator.Publish<DailyRoomServicesCreated>(
                                new DailyRoomServicesCreated(
                                                        allRoomsCovered: false,
                                                        numberOfServices: _createdServices,
                                                        new List<Guid>(),
                                                        _houseKeepersWithAssignment
                                                        ));
                            break;
                        }
                        //stuff that could be done here.. raise event stating
                        //no enough employees to service
                    }
                }

            }
        }
    }
}
