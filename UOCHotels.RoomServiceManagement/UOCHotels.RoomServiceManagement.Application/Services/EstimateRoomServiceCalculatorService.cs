using System;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.DomainServices;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Services
{
    public class EstimateRoomServiceCalculatorCommandHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoomRepository _roomRepository;

        public EstimateRoomServiceCalculatorCommandHandler(
                                    IEmployeeRepository employeeRepository,
                                    IRoomRepository roomRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        public async Task<int> Handle(RoomId roomId, EmployeeId employeeId)
        {
            var room = await _roomRepository.GetById(roomId);
            var employee = await _employeeRepository.GetById(employeeId);

            var service = new EstimateRoomServiceCalculator();
            return service.Calculate(room, employee);
        }
    }
}
